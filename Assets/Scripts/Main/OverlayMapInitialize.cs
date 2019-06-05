using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;

namespace Main
{
    public class OverlayMapInitialize : MonoBehaviour
    {
        private readonly RealCompass _compass = new RealCompass();

        public void Awake()
        {
            Input.compass.enabled = true;
            Input.location.Start();
        }

        public void Start()
        {
            StartCoroutine(WaitForCompassEnable());
        }

        private IEnumerator WaitForCompassEnable()
        {
            yield return new WaitUntil(() => _compass.IsEnabled);
            AlignMapWithCompass(_compass);
        }

        public void OnApplicationFocus(bool hasFocus)
        {
            StartCoroutine(WaitForCompassEnable());
        }

        public void AlignMapWithCompass(CompassInterface compass)
        {
            transform.rotation = compass.IsEnabled
                ? Quaternion.Euler(0,0, -compass.TrueHeading)
                : Quaternion.Euler(0, 0, 0);
        }
    }

    internal class RealCompass : CompassInterface
    {
        public bool IsEnabled => Math.Abs(TrueHeading) > 0;
        public float TrueHeading => Input.compass.trueHeading;
    }
}