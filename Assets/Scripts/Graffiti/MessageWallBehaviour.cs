using System;
using System.Threading.Tasks;
using Markers;
using UnityEngine;
using UnityEngine.UI;

namespace Graffiti
{
    public class MessageWallBehaviour : MonoBehaviour
    {
        public GameObject arCameraGameObject;
        public Text canvasText;
        public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        private Camera _arCameraComponent;
        private IoTMessageWall _ioTMessageWall;
        public TouchScreenKeyboard keyboard;
        private bool _keyboardOpened;

        public void Start()
        {
            _ioTMessageWall = new IoTMessageWall();
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
            if (TouchDetected(touchPosition))
            {
                keyboard = TouchScreenKeyboard.Open("");
                _keyboardOpened = true;
            }
        }

        private bool TouchDetected(Vector2 touchPosition)
        {
            var ray = _arCameraComponent.ScreenPointToRay(touchPosition);
            var (hitBehaviour, _) = physicsHandler.Raycast<MessageWallBehaviour>(ray);
            var touchDetected = Equals(this, hitBehaviour);
            return touchDetected;
        }

        private async void SendGraffitiTextToAws(string graffitiText)
        {
            await _ioTMessageWall.UpdateMessageWallText(graffitiText);
        }

        private async void PollForCanvasColorChange()
        {
            var textFromAws = await GetGraffitiTextFromAws();
            if (canvasText != null)
            {
                canvasText.text = textFromAws;
            }
        }

        private async Task<string> GetGraffitiTextFromAws()
        {
            if (_ioTMessageWall == null) return "No Graffiti Canvas";
            var state = await _ioTMessageWall.GetIoTThing();
            return state.text;
        }
    }
}