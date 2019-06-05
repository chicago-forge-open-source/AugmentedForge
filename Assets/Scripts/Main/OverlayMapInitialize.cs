using System;
using System.Collections;
using UnityEngine;

namespace Main
{
    public class OverlayMapInitialize : MonoBehaviour
    {
        public Camera mainCamera;
        public GameObject locationMarker;

        private readonly RealCompass _compass = new RealCompass();

        public void Awake()
        {
            Input.compass.enabled = true;
            Input.location.Start();
        }

        public void Start()
        {
            LocationSync(GameObject.Find("Sync Point 1"));
            StartCoroutine(WaitForCompassEnable());
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

        public void AlignCameraWithCompass(CompassInterface compass)
        {
            mainCamera.transform.rotation = compass.IsEnabled
                ? Quaternion.Euler(0, 0, -compass.TrueHeading)
                : Quaternion.Euler(0, 0, 0);
        }

        private void SetLocationMarkerPosition(Vector3 position)
        {
            locationMarker.transform.position = position;
        }
        
        private void SetCameraXyPosition(float x, float y)
        {
            var cameraTransform = mainCamera.transform;
            var camPosition = new Vector3(x, y, cameraTransform.position.z);
            cameraTransform.position = camPosition;
        }

        public void LocationSync(GameObject syncPoint)
        {
            var syncPos = syncPoint.transform.position;
            SetLocationMarkerPosition(syncPos);
            SetCameraXyPosition(syncPos.x, syncPos.y);
        }
    }

    internal class RealCompass : CompassInterface
    {
        public bool IsEnabled => Math.Abs(TrueHeading) > 0;
        public float TrueHeading => Input.compass.trueHeading;
    }
}