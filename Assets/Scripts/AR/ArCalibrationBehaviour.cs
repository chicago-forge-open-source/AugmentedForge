using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace AR
{
    public class ArCalibrationBehaviour : MonoBehaviour
    {
        public GameObject arCameraGameObject;
        public Text debugText;
        public GameObject startPoint;
        public GameObject arSessionOrigin;
        public ICompass compass = new RealCompass();
        public GameObject scrollItemPrefab;
        public GameObject scrollContent;

        public void Start()
        {
            SetStartPositionBasedOnSyncPoint();
            CreateScrollViewItems();
        }

        private void SetStartPositionBasedOnSyncPoint()
        {
            if (PlayerSelections.startingParametersProvided)
            {
                startPoint.transform.position = PlayerSelections.startingPoint;

                SetArSessionOriginPositionAndOrientation(PlayerSelections.startingPoint.x, PlayerSelections.startingPoint.z, PlayerSelections.orientation);
            }
            else
            {
                var syncPoint = Repositories.SyncPointRepository.Get()[0];
                startPoint.transform.position = new Vector3(syncPoint.X, 0, syncPoint.Z);
            
                SetArSessionOriginPositionAndOrientation(syncPoint.X, syncPoint.Z, compass.TrueHeading);
            }
        }

        private void SetArSessionOriginPositionAndOrientation(float newX, float newZ, float newOrientation)
        {
            Helpers.SetObjectXzPosition(arSessionOrigin.transform, newX, newZ);
            arSessionOrigin.transform.rotation = Quaternion.Euler(0, newOrientation, 0);
        }

        private void CreateScrollViewItems()
        {
            var syncPoints = Repositories.SyncPointRepository.Get();

            foreach (var syncPoint in syncPoints)
            {
                var clonedMarker = Instantiate(scrollItemPrefab, scrollContent.transform);
                clonedMarker.name = "ScrollItem-" + syncPoint.Name;
                clonedMarker.GetComponentInChildren<Text>().text = syncPoint.Name;
            }
        }

        public void Update()
        {
            var logLine = $"ARCamera: {arCameraGameObject.transform.position}";
            debugText.text = logLine;
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