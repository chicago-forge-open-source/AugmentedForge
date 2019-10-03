using Markers;
using UnityEngine;

namespace Roads
{
    public class RoadPoint
    {
        public RoadPoint(int position, Marker marker)
        {
            Position = position;
            Vector = new Vector3(marker.X, 0.0f, marker.Z);
        }

        public int Position { get; }

        public Vector3 Vector { get; }
    }
}