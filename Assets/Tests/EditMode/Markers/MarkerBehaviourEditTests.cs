using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Markers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Tests.EditMode.Markers
{
    public class MarkerBehaviourEditTests
    {
        private GameObject _game;
        private MarkerBehaviour _markerBehaviour;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _markerBehaviour = _game.AddComponent<MarkerBehaviour>();
            _markerBehaviour.ArCameraGameObject = new GameObject();
            _markerBehaviour.ArMarkerPrefab = new GameObject();
            _markerBehaviour.ArMarkerPrefab.AddComponent<Text>();
            _markerBehaviour.MapMarkerPrefab = new GameObject();
            _markerBehaviour.MapMarkerPrefab.AddComponent<Text>();
        }

        [Test]
        public void Start_MarkersAreDuplicatedAcrossLists()
        {
            Repositories.MarkerRepository.Save(
                new[] {new Marker("north", 1, 0), new Marker("west", 0, 1)}
            );

            _markerBehaviour.Start();

            Assert.True(_markerBehaviour.ArMarkers.First(marker => marker.name.Equals("north")));
            Assert.True(_markerBehaviour.MapMarkers.First(marker => marker.name.Equals("north")));
        }

        [Test]
        public void Update_MarkerBehaviorsIsConnectedCorrectly()
        {
            Repositories.MarkerRepository.Save(
                new[] {new Marker("north", 1, 0), new Marker("west", 0, 1)}
            );

            _markerBehaviour.Start();

            Assert.DoesNotThrow(() =>
            {
                _markerBehaviour.Update();
                UpdateMarkerBehaviours();
            });
        }

        private void UpdateMarkerBehaviours()
        {
            _markerBehaviour.ArMarkers.ForEach(gameObject =>
            {
                gameObject.GetComponent<MarkerFaceCameraBehaviour>().Update();
                gameObject.GetComponent<MarkerDistanceBehaviour>().Update();
            });
        }
    }
}