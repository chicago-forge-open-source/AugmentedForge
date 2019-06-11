using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.UI;

namespace AugmentedForge
{
    public class OverlayMapInitialize : MonoBehaviour
    {
        public Camera mainCamera;
        public Camera arCamera;
        public GameObject locationMarker;
        public Text debugText;
        public GameObject startPoint;

        private readonly RealCompass _compass = new RealCompass();
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
            var arCameraPosition = arCamera.transform.position;
            debugText.text = arCameraPosition.ToString();

            var startPointPosition = startPoint.transform.position;
            var locationMarkerX = startPointPosition.x + arCameraPosition.x;
            var locationMarkerY = startPointPosition.y + arCameraPosition.z;
            locationMarker.transform.position = new Vector3(locationMarkerX, locationMarkerY);
        }

        public void OnApplicationFocus(bool hasFocus)
        {
            StartCoroutine(WaitForCompassEnable());
        }

        private IEnumerator WaitForCompassEnable()
        {
            yield return new WaitUntil(() => _compass.IsEnabled);
            AlignCameraWithCompass(_compass);
        }

        public void AlignCameraWithCompass(ICompassInterface compass)
        {
            mainCamera.transform.rotation = compass.IsEnabled
                ? Quaternion.Euler(0, 0, -compass.TrueHeading)
                : Quaternion.Euler(0, 0, 0);
        }

        public void LocationSync(GameObject syncPoint)
        {
            var syncPos = syncPoint.transform.position;
            SetObjectXyPosition(locationMarker.transform, syncPos.x, syncPos.y);
            SetObjectXyPosition(mainCamera.transform, syncPos.x, syncPos.y);
        }

        private void SetObjectXyPosition(Transform objectTransform, float x, float y)
        {
            var position = new Vector3(x, y, objectTransform.position.z);
            objectTransform.position = position;
        }
    }

    internal class RealCompass : ICompassInterface
    {
        public bool IsEnabled => Math.Abs(TrueHeading) > 0;
        public float TrueHeading => Input.compass.trueHeading;
    }
}