using UnityEngine;

namespace Main
{
    public class OverlayMapInitialize : MonoBehaviour
    {
        // Start is called before the first frame update
        public void Start()
        {
            AlignMapWithCompass(new RealCompass());
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void AlignMapWithCompass(CompassInterface compass)
        {
            transform.rotation = compass.IsEnabled
                ? Quaternion.Euler(0, -compass.TrueHeading, 0)
                : Quaternion.Euler(0, 0, 0);
        }
    }

    internal class RealCompass : CompassInterface
    {
        public bool IsEnabled => Input.compass.enabled;
        public float TrueHeading => Input.compass.trueHeading;
    }
}