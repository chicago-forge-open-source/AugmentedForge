using Assets.Scripts;
using Assets.Scripts.Markers;
using Assets.Scripts.Roads;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Tests.EditMode
{
    public class YellowBrickRoadEditTests
    {
        [Test]
        public void SetPath_SetsRoadPositionPoints()
        {
            var yellowBrick = new GameObject();
            var script = yellowBrick.AddComponent<YellowBrickRoad>();

            var start = new RoadPoint(0, new Marker("Start", 30f, -2));
            var corner1 = new RoadPoint(1, new Marker("Corner 1", 24, -3));
            var corner2 = new RoadPoint(2, new Marker("Corner 2", 20f, -3.5f));
            var end = new RoadPoint(3, new Marker("End", 10f, -4));

            script.PathToDraw(new[] {start, corner1, corner2, end});
            var road = yellowBrick.GetComponent<LineRenderer>();
            
            Assert.AreEqual(4, road.positionCount);
            Assert.AreEqual(start.Vector, road.GetPosition(0));
            Assert.AreEqual(corner1.Vector, road.GetPosition(1));
            Assert.AreEqual(corner2.Vector, road.GetPosition(2));
            Assert.AreEqual(end.Vector, road.GetPosition(3));
        }
    }
}