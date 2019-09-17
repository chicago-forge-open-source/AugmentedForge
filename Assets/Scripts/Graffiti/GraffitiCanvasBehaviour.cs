using System;
using System.Threading.Tasks;
using Markers;
using UnityEngine;
using UnityEngine.UI;

namespace Graffiti
{
    public class GraffitiCanvasBehaviour : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public GameObject arCameraGameObject;
        public Text canvasText;
        public InputHandler inputHandler = new UnityInputHandler();
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        private Camera _arCameraComponent;
        private GraffitiCanvas _graffitiCanvas;
        private Boolean isRed;
        private TouchScreenKeyboard _keyboard;

        public void Start()
        {
            _graffitiCanvas = new GraffitiCanvas();
            _arCameraComponent = arCameraGameObject.GetComponent<Camera>();
            InvokeRepeating(nameof(PollForCanvasColorChange), 0.0f, 1f);
        }

        public void Update()
        {
            Debug.Log("+ Update GCB");

            if (_keyboard != null && _keyboard.status == TouchScreenKeyboard.Status.Done)
            {
                Debug.Log(canvasText);
                canvasText.text = _keyboard.text;
                _keyboard = null;
            }

            if (inputHandler.TouchCount <= 0) return;
            var touch = inputHandler.GetTouch(0);
            var touchPosition = _arCameraComponent.ScreenPointToRay(touch.position);
            if (Equals(this, physicsHandler.Raycast<GraffitiCanvasBehaviour>(touchPosition)))
            {
                _keyboard = TouchScreenKeyboard.Open("");

                if (!isRed)
                {
                    SendCanvasColorToAWS(Color.red);
                }
                else
                {
                    SendCanvasColorToAWS(Color.blue);
                }
            }
            Debug.Log("- Update Over GCB");
        }

        private void SendCanvasColorToAWS(Color color)
        {
            Task.Run(async () => { await _graffitiCanvas.UpdateGraffitiCanvasColor(color); })
                .GetAwaiter()
                .GetResult();
        }

        private void PollForCanvasColorChange()
        {
            meshRenderer.material.color = GetColorOfCanvas();
        }

        private Color GetColorOfCanvas()
        {
            if (_graffitiCanvas == null) return Color.magenta;
            var state = Task.Run(async () => await _graffitiCanvas.GetIoTThing()).GetAwaiter().GetResult();
            ColorUtility.TryParseHtmlString(state.color, out var color);
            isRed = color.Equals(Color.red);
            return color;
        }
    }
}