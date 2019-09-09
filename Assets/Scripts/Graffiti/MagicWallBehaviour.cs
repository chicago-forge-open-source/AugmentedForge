using System.Threading.Tasks;
using UnityEngine;

namespace Graffiti
{
    public class MagicWallBehaviour : MonoBehaviour
    {
        public MeshRenderer meshRenderer;

        public void Update()
        {
            meshRenderer.material.color = GetColorOfWall();
        }

        private static Color GetColorOfWall()
        {
            var state = Task.Run(async () => await WallSquare.GetIoTThing()).GetAwaiter().GetResult();
            ColorUtility.TryParseHtmlString(state.color, out var color);
            return color;
        }
    }
}