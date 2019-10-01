using System;
using Markers;
using UnityEngine;

namespace Graffiti
{
    public class GraffitiWallBehaviour : MonoBehaviour
    {
        public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        public Camera arCameraComponent;
        public Camera sketcherCamera;
        public Canvas hudCanvas;
        public Canvas sketcherUi;
        public Canvas dropGraffitiUi;
        public SketcherInputBehaviour sketcherInputBehaviour;
        public GameObject sketcherSurface;
        public DropGraffitiInputBehaviour dropGraffitiInputBehaviour;

        public void Start()
        {
            SwitchToARMode();
        }

        public void Update()
        {
            HandleTouch();
        }

        private void HandleTouch()
        {
            if (inputHandler.TouchCount > 0)
                HandleTouchAtPosition(inputHandler.GetTouch(0).position, EnableSketchMode);
        }
        
        
        private void EnableSketchMode()
        {
            hudCanvas.enabled = false;
            sketcherCamera.enabled = true;
            sketcherUi.enabled = true;
            sketcherInputBehaviour.enabled = true;
            sketcherSurface.SetActive(true);
            gameObject.SetActive(false);
        }

        private void SwitchToARMode()
        {
            dropGraffitiUi.enabled = false;
            dropGraffitiInputBehaviour.enabled = false;
            gameObject.SetActive(true);
            hudCanvas.enabled = true;
            sketcherCamera.enabled = false;
            sketcherUi.enabled = false;
            sketcherInputBehaviour.enabled = false;
            sketcherSurface.SetActive(false);
        }

        private void HandleTouchAtPosition(Vector2 touchPosition, Action callback)
        {
            if (TouchDetected(touchPosition))
            {
                callback();
            }
        }

        private bool TouchDetected(Vector2 touchPosition)
        {
            var ray = arCameraComponent.ScreenPointToRay(touchPosition);
            var (targetBehaviour, _) = physicsHandler.Raycast<GraffitiWallBehaviour>(ray);

            var touchDetected = Equals(this, targetBehaviour);
            return touchDetected;
        }

        public void ReturnToARMode()
        {
            SwitchToARMode();
        }

        public void SwitchToDropGraffitiMode()
        {
            sketcherCamera.enabled = true;
            gameObject.SetActive(true);
            dropGraffitiUi.enabled = true;
            dropGraffitiInputBehaviour.enabled = true;
        
            hudCanvas.enabled = false;
            sketcherSurface.SetActive(false);
            sketcherUi.enabled = false;
            sketcherInputBehaviour.enabled = false;
        }
    }
}