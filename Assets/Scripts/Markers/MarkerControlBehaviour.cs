using UnityEngine;

namespace Assets.Scripts.Markers
{
    public class MarkerControlBehaviour : MonoBehaviour
    {
        public InputHandler inputHandler = new UnityInputHandler();
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        public GameObject arCameraGameObject;
        public Marker marker;
        private Camera _arCameraComponent;
        private MarkerSpinBehaviour _spinBehaviour;
        private MarkerFaceCameraBehaviour _faceCameraBehaviour;
        private MarkerDistanceBehaviour _markerDistanceBehaviour;

        public void Start()
        {
            _arCameraComponent = arCameraGameObject.GetComponent<Camera>();
            _spinBehaviour = gameObject.AddComponent<MarkerSpinBehaviour>();
            _spinBehaviour.enabled = false;
            _spinBehaviour.Marker = marker;

            _faceCameraBehaviour = gameObject.AddComponent<MarkerFaceCameraBehaviour>();
            _faceCameraBehaviour.arCameraGameObject = arCameraGameObject;

            _markerDistanceBehaviour = gameObject.AddComponent<MarkerDistanceBehaviour>();
            _markerDistanceBehaviour.arCameraGameObject = arCameraGameObject;
        }

        public void Update()
        {
            if (inputHandler.TouchCount > 0)
            {
                var touch = inputHandler.GetTouch(0);
                var touchPosition = _arCameraComponent.ScreenPointToRay(touch.position);
                marker.Active = Equals(this, physicsHandler.Raycast<MarkerControlBehaviour>(touchPosition));
            }

            if (_spinBehaviour.RotatedFullCircle) marker.Active = false;
            _spinBehaviour.enabled = marker.Active;
            _faceCameraBehaviour.enabled = !marker.Active;
        }
    }
}