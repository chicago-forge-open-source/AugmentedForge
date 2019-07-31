using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Roads;
using UnityEngine;

namespace Assets.Scripts
{
    public class YellowBrickRoad : MonoBehaviour
    {
        public Material material;

        private LineRenderer NewRoad()
        {
            var road = gameObject.AddComponent<LineRenderer>();
            road.startWidth = 0.5f;
            road.endWidth = 0.5f;
            road.startColor = Color.yellow;
            road.endColor = Color.yellow;
            road.material = material;

            return road;
        }

        public void PathToDraw(IEnumerable<RoadPoint> vertices)
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