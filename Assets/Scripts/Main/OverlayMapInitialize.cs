using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;

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
            MoveLocationToSyncPoint(GameObject.Find("Sync Point 1"));
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
                ? Quaternion.Euler(0,0, -compass.TrueHeading)
                : Quaternion.Euler(0, 0, 0);
        }

        private void MoveLocationToSyncPoint(GameObject syncPoint)
        {
            locationMarker.transform.position = syncPoint.transform.position;
        }
    }

    internal class RealCompass : CompassInterface
    {
        public bool IsEnabled => Math.Abs(TrueHeading) > 0;
        public float TrueHeading => Input.compass.trueHeading;
    }
}