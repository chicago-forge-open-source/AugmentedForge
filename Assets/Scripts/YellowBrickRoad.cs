using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Roads;
using UnityEngine;

namespace Assets.Scripts
{
    public class YellowBrickRoad : MonoBehaviour
    {
        private LineRenderer NewRoad()
        {
            var road = gameObject.AddComponent<LineRenderer>();
            road.startWidth = 0.5f;
            road.endWidth = 0.5f;
            road.textureMode = LineTextureMode.Tile;
            road.material = Resources.Load<Material>("Materials/YellowBrickRoad");

            return road;
        }

        public void DrawPath(IEnumerable<RoadPoint> vertices)
        {
            var safeVertices = vertices.Where(vertex => vertex != null).ToArray();
            if (safeVertices.Length == 0) return;

            var road = NewRoad();
            road.positionCount = safeVertices.Length;
            foreach (var vertex in safeVertices)
            {
                road.SetPosition(vertex.Position, vertex.Vector);
            }
        }
    }
}