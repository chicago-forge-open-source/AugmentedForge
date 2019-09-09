using System.Threading.Tasks;
using UnityEngine;

namespace Graffiti
{
    public class MagicWallBehaviour : MonoBehaviour
    {
        private GraffitiCanvas _graffitiCanvas;
        public MeshRenderer meshRenderer;

        public void Start()
        {
            _graffitiCanvas = new GraffitiCanvas();
        }

        public void Update()
        {
            meshRenderer.material.color = GetColorOfWall();
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