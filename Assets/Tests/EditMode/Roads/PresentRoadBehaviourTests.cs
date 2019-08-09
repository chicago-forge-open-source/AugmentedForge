using System.Linq;
using Markers;
using NUnit.Framework;
using Roads;
using UnityEngine;

namespace Tests.EditMode.Roads
{
    public class PresentRoadBehaviourTests
    {
        private GameObject _gameObject;
        private PresentRoadsBehaviour _presentRoadsBehaviour;
        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _presentRoadsBehaviour = _gameObject.AddComponent<PresentRoadsBehaviour>();
        }
        
        [Test]
        public void Start_WillInitializeAnArPathAndAMapPath()
        {
            var mockMarker = new Marker("", 0, 0);
            var roadPoint = new RoadPoint(0, mockMarker);
            var mockRoad = new Road(new[] {roadPoint});
            Repositories.RoadRepository.Save(new[] {mockRoad});

            _presentRoadsBehaviour.Start();

            var roads = GameObject.FindGameObjectsWithTag("Road");
            Assert.AreEqual(2, roads.Length);
            Assert.NotNull(roads.First(road => road.layer == 8));
            Assert.NotNull(roads.First(road => road.layer == 9));
        }
    }
}