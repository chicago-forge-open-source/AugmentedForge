using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Tests.EditMode
{
    public class MarkerBehaviourEditTests
    {
        private readonly QuaternionEqualityComparer _quaternionComparer = new QuaternionEqualityComparer(10e-6f);

        private GameObject _game;
        private MarkerBehaviour _markerBehaviour;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _markerBehaviour = _game.AddComponent<MarkerBehaviour>();
            _markerBehaviour.ArCameraComponent = new GameObject();
            _markerBehaviour.MarkerPrefab = new GameObject();
        }

        [Test]
        public void Update_RotateMarkersToFaceArCameraLocation_EvenThoughTheModelFacesBackwardNaturally()
        {
            Repositories.MarkerRepository.Save(
                new[] {new Marker("north", 1, 0), new Marker("west", 0, 1)}
            );
            _markerBehaviour.Start();
            _markerBehaviour.ArCameraComponent.transform.position = new Vector3(0, 0, 0);

            _markerBehaviour.Update();

            var westMarkerGameObject = _markerBehaviour.Markers.First(marker => marker.name.Equals("west"));
            
            Assert.That(westMarkerGameObject
                    .transform.rotation,
                Is.EqualTo(Quaternion.Euler(0, 0, 0)).Using(_quaternionComparer)
            );

            var northMarkerGameObject = _markerBehaviour.Markers.First(marker => marker.name.Equals("north"));
            
            Assert.That(northMarkerGameObject
                    .transform.rotation,
                Is.EqualTo(Quaternion.Euler(0, 90, 0)).Using(_quaternionComparer)
            );
        }
    }
}