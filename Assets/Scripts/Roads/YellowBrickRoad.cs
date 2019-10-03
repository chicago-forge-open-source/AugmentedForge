using UnityEngine;

namespace Roads
{
    public class YellowBrickRoad : MonoBehaviour
    {
        private LineRenderer NewRoad()
        {
            var road = gameObject.AddComponent<LineRenderer>();
            road.alignment = LineAlignment.TransformZ;
            road.transform.rotation = Quaternion.Euler(90, 0, 0);
            road.widthMultiplier = 0.5f;
            road.textureMode = LineTextureMode.Tile;
            road.material = Resources.Load<Material>("Materials/YellowBrickRoad");
            return road;
        }

        public void DrawPath(RoadPoint[] vertices)
        {
            var road = NewRoad();
            road.positionCount = vertices.Length;
            foreach (var vertex in vertices)
            {
                road.SetPosition(vertex.Position, vertex.Vector);
            }
        }
    }
}