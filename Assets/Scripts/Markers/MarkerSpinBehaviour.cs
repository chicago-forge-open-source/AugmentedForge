using UnityEngine;

namespace Assets.Scripts.Markers
{
    public class MarkerSpinBehaviour : MonoBehaviour
    {
        private const int FramesPerSecond = 30;
        private const int RotationAmountPerFrame = 360 / FramesPerSecond;
        public Marker Marker;
        public bool RotatedFullCircle;
        public int RotationCount;

        public void OnEnable()
        {
            RotatedFullCircle = false;
            RotationCount = 0;
        }

        public void Update()
        {
            if (Marker.Active)
            {
                transform.Rotate(0, RotationAmountPerFrame, 0);

                RotationCount++;
                if (RotationCount == FramesPerSecond)
                {
                    RotatedFullCircle = true;
                }
            }

        }

        public void OnDisable()
        {
            RotatedFullCircle = false;
            RotationCount = 0;
        }
    }
}