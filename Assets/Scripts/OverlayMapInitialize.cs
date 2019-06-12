using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.UI;

namespace AugmentedForge
{
    public class OverlayMapInitialize : MonoBehaviour
    {
        public Camera mapCamera;
        public Camera arCamera;
        public GameObject locationMarker;
        public Text debugText;
        public GameObject startPoint;

        public ICompass compass = new RealCompass();
        public Vector3 CameraPrevPosition { get; set; }

        public void Awake()
        {
            Input.compass.enabled = true;
            Input.location.Start();
        }

        public void Start()
        {
            LocationSync(startPoint);
            StartCoroutine(WaitForCompassEnable());
        }

        public void Update()
        {
            UpdateLocationMarker();
            UpdateMapCamera();
        }

        private void UpdateMapCamera()
        {
            var divisor = 4f;
            var compassHeading = 360 - compass.TrueHeading;
            var mapCameraVector = mapCamera.transform.rotation.eulerAngles;
            var rotationDifference = CalculateRotationDifference(compassHeading, mapCameraVector);

            var finalRotation = mapCameraVector.z + rotationDifference / divisor;
            mapCamera.transform.rotation = Quaternion.Euler(0, 0, finalRotation);
            var logLine = "Current:" + Math.Round(mapCamera.transform.rotation.eulerAngles.z)
                                     + "\n" + "Compass:" + Math.Round(compassHeading)
                                     + "\n" + "Rotation difference:" + Math.Round(rotationDifference);
            debugText.text = logLine;
            Debug.Log(logLine);
        }

        private void UpdateLocationMarker()
        {
            var arCameraPosition = arCamera.transform.position;

            var startPointPosition = startPoint.transform.position;
            var locationX = startPointPosition.x + arCameraPosition.x;
            var locationY = startPointPosition.y + arCameraPosition.z;
            locationMarker.transform.position = new Vector3(locationX, locationY);
        }

        private static float CalculateRotationDifference(float compassHeading, Vector3 mapCameraVector)
        {
            var rotationDifference = compassHeading - mapCameraVector.z;

            if (rotationDifference > 180)
            {
                rotationDifference -= 360;
            }
            else if (rotationDifference < -180)
            {
                rotationDifference += 360;
            }

            return rotationDifference;
        }

        public void OnApplicationFocus(bool hasFocus)
        {
            StartCoroutine(WaitForCompassEnable());
        }

        private IEnumerator WaitForCompassEnable()
        {
            yield return new WaitUntil(() => compass.IsEnabled);
            AlignCameraWithCompass(compass);
        }

        public void AlignCameraWithCompass(ICompass theCompass)
        {
            mapCamera.transform.rotation = theCompass.IsEnabled
                ? Quaternion.Euler(0, 0, -theCompass.TrueHeading)
                : Quaternion.Euler(0, 0, 0);
        }

        public void LocationSync(GameObject syncPoint)
        {
            var syncPos = syncPoint.transform.position;
            SetObjectXyPosition(locationMarker.transform, syncPos.x, syncPos.y);
            SetObjectXyPosition(mapCamera.transform, syncPos.x, syncPos.y);
        }

        private void SetObjectXyPosition(Transform objectTransform, float x, float y)
        {
            var position = new Vector3(x, y, objectTransform.position.z);
            objectTransform.position = position;
        }
    }

    internal class RealCompass : ICompass
    {
        public bool IsEnabled => Math.Abs(TrueHeading) > 0;
        public float TrueHeading => Input.compass.trueHeading;
    }
}