﻿using System;
using System.Collections;
using SyncPoints;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace AR
{
    public class ArCalibrationBehaviour : MonoBehaviour
    {
        public GameObject arCameraGameObject;
        public Text debugText;
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

            resetSessionFunction(session);
        }

        public void Update()
        {
            var logLine = $"ARCamera: {arCameraGameObject.transform.position}";
            logLine += $"\nSessionOrigin: {arSessionOrigin.transform.position}";
            logLine += $"\nSessionOriginRotation: {arSessionOrigin.transform.rotation}";
            debugText.text = logLine;

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