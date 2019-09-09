using System.Threading.Tasks;
using Markers;
using UnityEngine;

namespace Graffiti
{
    public class MagicWallBehaviour : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public InputHandler inputHandler = new UnityInputHandler();
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        public GameObject arCameraGameObject;
        private Camera _arCameraComponent;
        private GraffitiCanvas _graffitiCanvas;

        public void Start()
        {
            _graffitiCanvas = new GraffitiCanvas();
            _arCameraComponent = arCameraGameObject.GetComponent<Camera>();
        }

        public void Update()
        {
            meshRenderer.material.color = GetColorOfWall();

            if (inputHandler.TouchCount <= 0) return;
            var touch = inputHandler.GetTouch(0);
            var touchPosition = _arCameraComponent.ScreenPointToRay(touch.position);
            if (Equals(this, physicsHandler.Raycast<MagicWallBehaviour>(touchPosition)))
            {
                Task.Run(async () => { await _graffitiCanvas.UpdateMagicWallColor(Color.red); })
                    .GetAwaiter()
                    .GetResult();
            }
        }

        private Color GetColorOfWall()
        {
            if (_graffitiCanvas == null) return Color.magenta;
            var state = Task.Run(async () => await _graffitiCanvas.GetIoTThing()).GetAwaiter().GetResult();
            ColorUtility.TryParseHtmlString(state.color, out var color);
            return color;
        }
    }
}