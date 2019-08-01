using System.Linq;
using Assets.Scripts;
using Markers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Tests.EditMode.Markers
{
    public class InitializeMarkersTests
    {
        private GameObject _game;
        private InitializeMarkers _initializeMarkers;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _initializeMarkers = _game.AddComponent<InitializeMarkers>();
            _initializeMarkers.arCameraGameObject = new GameObject();
            _initializeMarkers.arMarkerPrefab = new GameObject();
            _initializeMarkers.arMarkerPrefab.AddComponent<Text>();
            _initializeMarkers.mapMarkerPrefab = new GameObject();
            _initializeMarkers.mapMarkerPrefab.AddComponent<Text>();
        }

        [Test]
        public void Start_MarkersAreDuplicatedAcrossLists()
        {
            Repositories.MarkerRepository.Save(
                new[] {new Marker("north", 1, 0), new Marker("west", 0, 1)}
            );

            _initializeMarkers.Start();

            Assert.IsTrue(_initializeMarkers.ArMarkers.First(marker => marker.name.Equals("north")));
            Assert.IsTrue(_initializeMarkers.MapMarkers.First(marker => marker.name.Equals("north")));
        }

        [Test]
        public void Update_MarkerBehaviorsIsConnectedCorrectly()
        {
            Repositories.MarkerRepository.Save(
                new[] {new Marker("north", 1, 0), new Marker("west", 0, 1)}
            );

            _initializeMarkers.Start();

            Assert.DoesNotThrow(() =>
            {
                UpdateMarkerBehaviours2();
            });
        }

        private void UpdateMarkerBehaviours()
        {
            _initializeMarkers.ArMarkers.ForEach(gameObject =>
            {
                gameObject.GetComponent<MarkerFaceCameraBehaviour>().Update();
                gameObject.GetComponent<MarkerDistanceBehaviour>().Update();
            });
        }
        
        private void UpdateMarkerBehaviours2()
        {
            _initializeMarkers.ArMarkers.ForEach(gameObject =>
            {
                var markerControlBehaviour = gameObject.GetComponent<MarkerControlBehaviour>();
                markerControlBehaviour.Start();
                markerControlBehaviour.Update();
            });
        }
    }
}