using System.Linq;
using Markers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Tests.EditMode.Markers
{
    public class PresentMarkersTests
    {
        private GameObject _game;
        private PresentMarkersBehaviour _presentMarkersBehaviour;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _presentMarkersBehaviour = _game.AddComponent<PresentMarkersBehaviour>();
            _presentMarkersBehaviour.arCameraGameObject = new GameObject();
            _presentMarkersBehaviour.arMarkerPrefab = new GameObject();
            _presentMarkersBehaviour.arMarkerPrefab.AddComponent<Text>();
            _presentMarkersBehaviour.mapMarkerPrefab = new GameObject();
            _presentMarkersBehaviour.mapMarkerPrefab.AddComponent<Text>();
        }

        [Test]
        public void Start_MarkersAreDuplicatedAcrossLists()
        {
            Repositories.MarkerRepository.Save(
                new[] {new Marker("north", 1, 0), new Marker("west", 0, 1)}
            );

            _presentMarkersBehaviour.Start();

            Assert.IsTrue(_presentMarkersBehaviour.ArMarkers.First(marker => marker.name.Equals("north")));
            Assert.IsTrue(_presentMarkersBehaviour.MapMarkers.First(marker => marker.name.Equals("north")));
        }

        [Test]
        public void Update_MarkerBehaviorsIsConnectedCorrectly()
        {
            Repositories.MarkerRepository.Save(
                new[] {new Marker("north", 1, 0), new Marker("west", 0, 1)}
            );

            _presentMarkersBehaviour.Start();

            Assert.DoesNotThrow(UpdateMarkerBehaviours);
        }

        private void UpdateMarkerBehaviours()
        {
            _presentMarkersBehaviour.ArMarkers.ForEach(gameObject =>
            {
                var markerControlBehaviour = gameObject.GetComponent<MarkerControlBehaviour>();
                markerControlBehaviour.Start();
                markerControlBehaviour.Update();
            });
        }
    }
}