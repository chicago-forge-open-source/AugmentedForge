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
            var spinBehaviour = gameObject.AddComponent<MarkerSpinBehaviour>();
            spinBehaviour.enabled = false;
            spinBehaviour.Marker = Marker;

            var faceCameraBehaviour = gameObject.AddComponent<MarkerFaceCameraBehaviour>();
            faceCameraBehaviour.ArCameraGameObject = ArCameraGameObject;
        }

        public void Update()
        {
            if (InputHandler.TouchCount > 0)
            {
                var touch = InputHandler.GetTouch(0);
                var touchPosition = ArCameraGameObject.GetComponent<Camera>().ScreenPointToRay(touch.position);
                
                if (Equals(this, PhysicsHandler.Raycast<MarkerControlBehaviour>(touchPosition)))
                {
                    Marker.Active = true;
                }
            }
            
            if (Marker.Active)
            {
                EnableMarkerActiveBehaviors();
                if (GetComponent<MarkerSpinBehaviour>().RotatedFullCircle)
                {
                    Marker.Active = false;
                    DisableMarkerActiveBehaviors();
                }
            }
            else
            {
                DisableMarkerActiveBehaviors();
            }
        }

        private void EnableMarkerActiveBehaviors()
        {
            GetComponent<MarkerSpinBehaviour>().enabled = true;
            GetComponent<MarkerFaceCameraBehaviour>().enabled = false;
        }

        private void DisableMarkerActiveBehaviors()
        {
            GetComponent<MarkerSpinBehaviour>().enabled = false;
            GetComponent<MarkerFaceCameraBehaviour>().enabled = true;
        }
    }
}