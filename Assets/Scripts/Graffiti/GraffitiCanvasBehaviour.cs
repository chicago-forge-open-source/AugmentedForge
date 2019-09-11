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
        public InputHandler inputHandler = new UnityInputHandler();
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        public GameObject arCameraGameObject;
        private Camera _arCameraComponent;
        private GraffitiCanvas _graffitiCanvas;
        private Boolean isRed;

        public void Start()
        {
            _graffitiCanvas = new GraffitiCanvas();
            _arCameraComponent = arCameraGameObject.GetComponent<Camera>();
            InvokeRepeating(nameof(PollForCanvasColorChange), 0.0f, 1f);
        }

        public void Update()
        {
            if (inputHandler.TouchCount <= 0) return;
            var touch = inputHandler.GetTouch(0);
            var touchPosition = _arCameraComponent.ScreenPointToRay(touch.position);
            if (Equals(this, physicsHandler.Raycast<GraffitiCanvasBehaviour>(touchPosition)))
            {
                if (!isRed)
                {
                    Task.Run(async () => { await _graffitiCanvas.UpdateGraffitiCanvasColor(Color.red); })
                        .GetAwaiter()
                        .GetResult();
                }
                else
                {
                    Task.Run(async () => { await _graffitiCanvas.UpdateGraffitiCanvasColor(Color.blue); })
                        .GetAwaiter()
                        .GetResult();
                }
            }
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
