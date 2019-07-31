using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Markers;
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
            _initializeMarkers.ArCameraGameObject = new GameObject();
            _initializeMarkers.ArMarkerPrefab = new GameObject();
            _initializeMarkers.ArMarkerPrefab.AddComponent<Text>();
            _initializeMarkers.MapMarkerPrefab = new GameObject();
            _initializeMarkers.MapMarkerPrefab.AddComponent<Text>();
        }

        [Test]
        public void Start_MarkersAreDuplicatedAcrossLists()
        {
            Repositories.MarkerRepository.Save(
                new[] {new Marker("north", 1, 0), new Marker("west", 0, 1)}
            );

            _initializeMarkers.Start();

            Assert.True(_initializeMarkers.ArMarkers.First(marker => marker.name.Equals("north")));
            Assert.True(_initializeMarkers.MapMarkers.First(marker => marker.name.Equals("north")));
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
                _initializeMarkers.Update();
                UpdateMarkerBehaviours();
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
    }
}