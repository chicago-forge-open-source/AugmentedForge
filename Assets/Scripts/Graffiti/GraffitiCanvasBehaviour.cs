using System;
using System.Threading.Tasks;
using Markers;
using UnityEngine;
using UnityEngine.UI;

namespace Graffiti
{
    public class GraffitiCanvasBehaviour : MonoBehaviour
    {
        public GameObject arCameraGameObject;
        public Text canvasText;
        public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        private Camera _arCameraComponent;
        private GraffitiCanvas _graffitiCanvas;
        public TouchScreenKeyboard keyboard;
        private bool _keyboardOpened;

        public void Start()
        {
            _graffitiCanvas = new GraffitiCanvas();
            _arCameraComponent = arCameraGameObject.GetComponent<Camera>();
            InvokeRepeating(nameof(PollForCanvasColorChange), 0.0f, 1f);
        }

        public void Update()
        {
            if (keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done && _keyboardOpened)
            {
                SendGraffitiTextToAws(keyboard.text);
                keyboard = null;
                _keyboardOpened = false;
            }

            HandleTouch();
        }

        private void HandleTouch()
        {
            if (inputHandler.TouchCount > 0) HandleTouchAtPosition(inputHandler.GetTouch(0).position);
        }

        private void HandleTouchAtPosition(Vector2 touchPosition)
        {
            var ray = _arCameraComponent.ScreenPointToRay(touchPosition);
            if (Equals(this, physicsHandler.Raycast<GraffitiCanvasBehaviour>(ray)))
            {
                keyboard = TouchScreenKeyboard.Open("");
                _keyboardOpened = true;
            }
        }

        private void SendGraffitiTextToAws(string graffitiText)
        {
            Task.Run(async () => { await _graffitiCanvas.UpdateGraffitiCanvasText(graffitiText); })
                .GetAwaiter()
                .GetResult();
        }

        private void PollForCanvasColorChange()
        {
            canvasText.text = GetGraffitiTextFromAws();
        }

        private string GetGraffitiTextFromAws()
        {
            if (_graffitiCanvas == null) return "No Graffiti Canvas";
            var state = Task.Run(async () => await _graffitiCanvas.GetIoTThing()).GetAwaiter().GetResult();
            return state.text;
        }
    }
}