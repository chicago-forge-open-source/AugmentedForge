using System;
using System.Collections;
using SyncPoints;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace AR
{
    public class ArCalibrationBehaviour : MonoBehaviour
    {
        public GameObject arCameraGameObject;
        public GameObject arSessionOrigin;
        public ICompass compass = new RealCompass();
        public SyncPoint pendingSyncPoint;
        public ARSession session;
        public Action<ARSession> resetSessionFunction = session => session.Reset();

        public void Start()
        {
            SetStartPositionBasedOnSyncPoint();
        }

        private void SetStartPositionBasedOnSyncPoint()
        {
            var repoSyncPoint = Repositories.SyncPointRepository.Get()[0];
            if (PlayerPrefs.GetString("location").Equals("GrandOpening"))
            {
                repoSyncPoint = new SyncPoint("GrandOpening", 32f, -16, 0);
            }
            pendingSyncPoint = new SyncPoint(
                "start with compass",
                repoSyncPoint.X,
                repoSyncPoint.Z,
                compass.TrueHeading
            );
        }

        private void SetArSessionOriginPositionAndOrientation(float newX, float newZ, float newOrientation)
        {
            Helpers.SetObjectXzPosition(arSessionOrigin.transform, newX, newZ);
            arSessionOrigin.transform.rotation = Quaternion.Euler(0, newOrientation, 0);

            // This super messes up the way tracked image distance from camera is measured
            resetSessionFunction(session);
        }

        public void Update()
        {
            if (pendingSyncPoint != null)
            {
                SetArSessionOriginPositionAndOrientation(pendingSyncPoint.X, pendingSyncPoint.Z,
                    pendingSyncPoint.Orientation);
                pendingSyncPoint = null;
            }
        }

        public void OnApplicationFocus(bool hasFocus)
        {
            StartCoroutine(WaitForCompassEnable());
        }

        private IEnumerator WaitForCompassEnable()
        {
            yield return new WaitUntil(() => compass.IsEnabled);
        }
    }

    internal class RealCompass : ICompass
    {
        public bool IsEnabled => Math.Abs(TrueHeading) > 0;
        public float TrueHeading => Input.compass.trueHeading;
    }
    
    public static class Helpers
    {
        public static void SetObjectXzPosition(Transform objectTransform, float x, float z)
        {
            var position = new Vector3(x, objectTransform.position.y, z);
            objectTransform.position = position;
        }
    }
}