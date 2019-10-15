using System;
using UnityEngine;

namespace Markers
{
    public class MarkerSpinBehaviour : MonoBehaviour
    {
        private const double SlowdownScale = 2.2;
        private const int FramesPerSecond = 30;
        private static readonly int RotationAmountPerFrame = (int) Math.Round(360 / FramesPerSecond / SlowdownScale);
        public Marker marker;
        public bool rotatedFullCircle;
        public int rotationCount;

        public void OnEnable()
        {
            rotatedFullCircle = false;
            rotationCount = 0;
        }

        public void OnMouseUp()
        {
            gameObject.GetComponent<AudioSource>().Play();
        }

        public void Update()
        {
            if (!marker.Active) return;
            transform.Rotate(0, RotationAmountPerFrame, 0);
            rotationCount++;
            rotatedFullCircle = rotationCount == Math.Round(FramesPerSecond * SlowdownScale);
        }

        public void OnDisable()
        {
            rotatedFullCircle = false;
            rotationCount = 0;
        }
    }
}