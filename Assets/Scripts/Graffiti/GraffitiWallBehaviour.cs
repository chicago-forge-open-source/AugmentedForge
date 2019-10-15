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
        public GameObject hudCanvas;
        public GameObject sketcherUi;
        public GameObject dropGraffitiUi;
        public GameObject sketcherSurface;
        public DropGraffitiInputBehaviour dropGraffitiInputBehaviour;
        public GameObject iotLight;
        public GameObject avocado;
        public GameObject messageWall;

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


        public void EnableSketchMode()
        {
            enabled = false;
            gameObject.SetActive(false);
            hudCanvas.SetActive(false);
            dropGraffitiUi.SetActive(false);
            iotLight.SetActive(false);
            avocado.SetActive(false);
            messageWall.SetActive(false);
            dropGraffitiInputBehaviour.enabled = false;

            sketcherCamera.enabled = true;
            sketcherUi.SetActive(true);
            sketcherSurface.SetActive(true);
        }

        private void SwitchToARMode()
        {
            enabled = true;
            gameObject.SetActive(true);
            hudCanvas.SetActive(true);
            iotLight.SetActive(true);
            avocado.SetActive(true);
            messageWall.SetActive(true);

            dropGraffitiUi.SetActive(false);
            dropGraffitiInputBehaviour.enabled = false;
            sketcherCamera.enabled = false;
            sketcherUi.SetActive(false);
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
            enabled = false;
            sketcherCamera.enabled = true;
            gameObject.SetActive(true);
            dropGraffitiUi.SetActive(true);
            dropGraffitiInputBehaviour.enabled = true;

            hudCanvas.SetActive(false);
            sketcherSurface.SetActive(false);
            sketcherUi.SetActive(false);
        }
    }
}