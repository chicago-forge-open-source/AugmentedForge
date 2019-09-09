using System.Threading.Tasks;
using UnityEngine;

namespace Graffiti
{
    public class MagicWallBehaviour : MonoBehaviour
    {
        private WallSquare _wallSquare;
        public MeshRenderer meshRenderer;

        public void Start()
        {
            _wallSquare = new WallSquare();
        }

        public void Update()
        {
            meshRenderer.material.color = GetColorOfWall();
        }

        private Color GetColorOfWall()
        {
            if (_wallSquare == null) return Color.magenta;
            var state = Task.Run(async () => await _wallSquare.GetIoTThing()).GetAwaiter().GetResult();
            ColorUtility.TryParseHtmlString(state.color, out var color);
            return color;
        }
    }
}