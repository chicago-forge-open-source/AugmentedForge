using System;
using System.Collections;
using AugmentedForge;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class OverlayMapInitialize : MonoBehaviour
    {
        public Camera MapCamera;
        public Camera ArCamera;
        public GameObject LocationMarker;
        public Text DebugText;
        public GameObject StartPoint;
        public GameObject ArSessionOrigin;
        public ICompass Compass = new RealCompass();

        public void Awake()
        {
            Input.compass.enabled = true;
            Input.location.Start();
        }

        public void Start()
        {
            LocationSync(StartPoint);
            StartCoroutine(WaitForCompassEnable());
            ArSessionOrigin.transform.rotation = Quaternion.Euler(0, Compass.TrueHeading, 0);
        }

        public void Update()
        {
            UpdateLocationMarker();
            UpdateMapCamera();
        }

        public void OnApplicationFocus(bool hasFocus)
        {
            StartCoroutine(WaitForCompassEnable());
        }
        
        private void UpdateMapCamera()
        {
            var divisor = 4f;
            var compassHeading = 360 - Compass.TrueHeading;
            var mapCameraVector = MapCamera.transform.rotation.eulerAngles;
            var rotationDifference = CalculateRotationDifference(compassHeading, mapCameraVector);

            var finalRotation = mapCameraVector.z + rotationDifference / divisor;
            MapCamera.transform.rotation = Quaternion.Euler(0, 0, finalRotation);
        }

        private void UpdateLocationMarker()
        {
            var arCameraPosition = ArCamera.transform.position;

            var startPointPosition = StartPoint.transform.position;
            var locationX = startPointPosition.x + arCameraPosition.x;
            var locationY = startPointPosition.y + arCameraPosition.z;
            LocationMarker.transform.position = new Vector3(locationX, locationY);

            var logLine = "ARCamera: " + arCameraPosition;
            DebugText.text = logLine;
            Debug.Log(logLine);
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


        private IEnumerator WaitForCompassEnable()
        {
            yield return new WaitUntil(() => Compass.IsEnabled);
        }

        private void LocationSync(GameObject syncPoint)
        {
            var syncPos = syncPoint.transform.position;
            SetObjectXyPosition(LocationMarker.transform, syncPos.x, syncPos.y);
            SetObjectXyPosition(MapCamera.transform, syncPos.x, syncPos.y);
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