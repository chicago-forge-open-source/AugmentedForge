using UnityEngine;

namespace Assets.Scripts.Markers
{
    public class MarkerSpinBehaviour : MonoBehaviour
    {
        public void Update()
        {
            transform.Rotate(0,12, 0);
        }
    }
}