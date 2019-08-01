using Markers;
using NUnit.Framework;
using Roads;
using UnityEngine;

namespace Tests.EditMode
{
    public class YellowBrickRoadEditTests
    {
        private GameObject _yellowBrick;
        private YellowBrickRoad _brickScript;
        
        [SetUp]
        public void SetUp()
        {
            _yellowBrick = new GameObject();
            _brickScript = _yellowBrick.AddComponent<YellowBrickRoad>();
        }

        [Test]
        public void SetPath_SetsRoadPositionPoints()
        {
            var start = new RoadPoint(0, new Marker("Start", 30f, -2));
            var corner1 = new RoadPoint(1, new Marker("Corner 1", 24, -3));
            var corner2 = new RoadPoint(2, new Marker("Corner 2", 20f, -3.5f));
            var end = new RoadPoint(3, new Marker("End", 10f, -4));

            _brickScript.DrawPath(new[] {start, corner1, corner2, end});
            var road = _yellowBrick.GetComponent<LineRenderer>();
            
            Assert.AreEqual(4, road.positionCount);
            Assert.AreEqual(start.Vector, road.GetPosition(0));
            Assert.AreEqual(corner1.Vector, road.GetPosition(1));
            Assert.AreEqual(corner2.Vector, road.GetPosition(2));
            Assert.AreEqual(end.Vector, road.GetPosition(3));
        }
    }
}