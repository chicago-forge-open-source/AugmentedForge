using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Roads;
using Markers;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Tests.EditMode.Roads
{
    public class RoadBehaviourEditTests
    {
        private GameObject _gameObject;
        private RoadBehaviour _roadBehaviour;
        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _roadBehaviour = _gameObject.AddComponent<RoadBehaviour>();
        }
        
        [Test]
        public void Start_WillInitializeAnArPathAndAMapPath()
        {
            var mockMarker = new Marker("", 0, 0);
            var roadPoint = new RoadPoint(0, mockMarker);
            var mockRoad = new Road(new[] {roadPoint});
            Repositories.RoadRepository.Save(new[] {mockRoad});

            _roadBehaviour.Start();

            var roads = GameObject.FindGameObjectsWithTag("Road");
            Assert.AreEqual(2, roads.Length);
            Assert.NotNull(roads.First(road => road.layer == 8));
            Assert.NotNull(roads.First(road => road.layer == 9));
        }
    }
}