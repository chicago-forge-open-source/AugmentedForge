using System;
using UnityEngine;

namespace Markers
{
    public class MarkerControlBehaviour : MonoBehaviour
    {
        public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
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
            _spinBehaviour.marker = marker;

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
                var (hitBehaviour, _) = physicsHandler.Raycast<MarkerControlBehaviour>(touchPosition);
                marker.Active = Equals(this, hitBehaviour);
            }

            if (_spinBehaviour.rotatedFullCircle) marker.Active = false;
            _spinBehaviour.enabled = marker.Active;
            _faceCameraBehaviour.enabled = !marker.Active;
        }
    }
}