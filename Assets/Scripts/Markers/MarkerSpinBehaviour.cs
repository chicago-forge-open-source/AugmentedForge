using UnityEngine;

namespace Assets.Scripts.Markers
{
    public class MarkerSpinBehaviour : MonoBehaviour
    {
        private const int FramesPerSecond = 30;
        private const int RotationAmountPerFrame = 360 / FramesPerSecond;
        public Marker Marker;
        public bool RotatedFullCircle = false;
        public int RotationCount;

        public void Update()
        {
            if (Marker.Active)
            {
                transform.Rotate(0, RotationAmountPerFrame, 0);

                RotationCount++;
                if (RotationCount == FramesPerSecond)
                {
                    RotatedFullCircle = true;
                    RotationCount = 0;
                }
            }
            else
            {
                RotatedFullCircle = false;
            }

        }
    }
}