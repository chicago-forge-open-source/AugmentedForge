using System.Collections.Generic;
using System.Linq;
using Roads;
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