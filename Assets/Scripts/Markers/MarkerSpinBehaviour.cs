using UnityEngine;

namespace Markers
{
    public class MarkerSpinBehaviour : MonoBehaviour
    {
        private const int FramesPerSecond = 30;
        private const int RotationAmountPerFrame = 360 / FramesPerSecond;
        public Marker marker;
        public bool rotatedFullCircle;
        public int rotationCount;

        public void OnEnable()
        {
            rotatedFullCircle = false;
            rotationCount = 0;
        }

        public void Update()
        {
            if (marker.Active)
            {
                transform.Rotate(0, RotationAmountPerFrame, 0);

                rotationCount++;
                if (rotationCount == FramesPerSecond)
                {
                    rotatedFullCircle = true;
                }
            }

        }

        public void OnDisable()
        {
            rotatedFullCircle = false;
            rotationCount = 0;
        }
    }
}