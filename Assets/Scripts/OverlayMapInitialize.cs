using UnityEditor;
using UnityEngine.Experimental.XR;
using UnityEngine.XR;
using System;
using System.Collections;
using UnityEngine;

namespace AugmentedForge
{
    public class OverlayMapInitialize : MonoBehaviour
    {
        public Camera mainCamera;
        public GameObject locationMarker;

        private readonly RealCompass _compass = new RealCompass();
        public Vector3 CameraPrevPosition { get; set; }

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

        public void MoveLocationMarker(Vector3 cameraPosition)
        {
            var deltaPosition = cameraPosition - CameraPrevPosition;
            SetObjectXyPosition(locationMarker.transform, deltaPosition.x, deltaPosition.y);
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