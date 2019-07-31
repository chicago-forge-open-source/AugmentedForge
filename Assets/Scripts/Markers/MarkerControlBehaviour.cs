using UnityEngine;

namespace Assets.Scripts.Markers
{
    public class MarkerControlBehaviour : MonoBehaviour
    {
        public InputHandler InputHandler = new UnityInputHandler();
        public PhysicsHandler PhysicsHandler = new UnityPhysicsHandler();
        public GameObject ArCameraGameObject;
        public Marker Marker;

        public void Start()
        {
            gameObject.AddComponent<MarkerSpinBehaviour>().enabled = false;

            var faceCameraBehaviour = gameObject.AddComponent<MarkerFaceCameraBehaviour>();
            faceCameraBehaviour.ArCameraGameObject = ArCameraGameObject;
        }

        public void Update()
        {
            if (Marker.Active)
            {
                GetComponent<MarkerSpinBehaviour>().enabled = true;
                GetComponent<MarkerFaceCameraBehaviour>().enabled = false;
            }
            else
            {
                GetComponent<MarkerSpinBehaviour>().enabled = false;
            }

            if (InputHandler.TouchCount > 0)
            {
                Marker.Active = true;
            }
        }
    }
}