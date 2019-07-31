using UnityEngine;

namespace Assets.Scripts.Markers
{
    public class MarkerSpinBehaviour : MonoBehaviour
    {
        private const int FramesPerSecond = 30;
        private const int RotationAmountPerFrame = 360 / FramesPerSecond;

        public void Update()
        {
            transform.Rotate(0,RotationAmountPerFrame, 0);
        }
    }
}