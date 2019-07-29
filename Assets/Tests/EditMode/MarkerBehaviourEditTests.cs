using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using UnityEngine.UI;

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
        public void Update_GivenTheModelFacesEastNaturally_RotateArMarkersToFaceArCameraLocation()
        {
            Repositories.MarkerRepository.Save(
                new[] {new Marker("north", 1, 0), new Marker("west", 0, 1)}
            );
            _markerBehaviour.Start();
            _markerBehaviour.ArCameraComponent.transform.position = new Vector3(0, 0, 0);

            _markerBehaviour.Update();

            var westMarkerGameObject = _markerBehaviour.ArMarkers.First(marker => marker.name.Equals("west"));

            Assert.That(westMarkerGameObject
                    .transform.rotation,
                Is.EqualTo(Quaternion.Euler(0, 180, 0)).Using(_quaternionComparer)
            );

            var northMarkerGameObject = _markerBehaviour.ArMarkers.First(marker => marker.name.Equals("north"));

            Assert.That(northMarkerGameObject
                    .transform.rotation,
                Is.EqualTo(Quaternion.Euler(0, 270, 0)).Using(_quaternionComparer)
            );
        }

        [Test]
        public void Update_ArMarkersDoNotChangeRotationOnXOrZAxises()
        {
            Repositories.MarkerRepository.Save(new[] {new Marker("north", 1, 0)});
            _markerBehaviour.Start();
            _markerBehaviour.ArCameraComponent.transform.position = new Vector3(0, 100, 0);

            _markerBehaviour.Update();

            var northMarkerGameObject = _markerBehaviour.ArMarkers.First(marker => marker.name.Equals("north"));

            Assert.That(northMarkerGameObject.transform.rotation,
                Is.EqualTo(Quaternion.Euler(0, 270, 0)).Using(_quaternionComparer)
            );
        }

        [Test]
        public void Update_GivenUserIsNotNearArMarkers_NoArMarkersAreShown()
        {
            Repositories.MarkerRepository.Save(new[] {new Marker("north", 11, 0)});
                        _markerBehaviour.Start();
                        _markerBehaviour.ArCameraComponent.transform.position = new Vector3(0, 0, 0);
            
                        _markerBehaviour.Update();
            
                        var northMarkerGameObject = _markerBehaviour.ArMarkers.First(marker => marker.name.Equals("north"));
            
                        Assert.False(northMarkerGameObject.activeSelf);
        }
        
        [Test]
        public void Update_GivenUserIsNearArMarkers_ArMarkersAreShown()
        {
            Repositories.MarkerRepository.Save(new[] {new Marker("north", 4, 0)});
            _markerBehaviour.Start();
            _markerBehaviour.ArCameraComponent.transform.position = new Vector3(0, 0, 0);
            
            _markerBehaviour.Update();
            
            var northMarkerGameObject = _markerBehaviour.ArMarkers.First(marker => marker.name.Equals("north"));
            
            Assert.True(northMarkerGameObject.activeSelf);
        }
        
        [Test]
        public void Update_GivenUserIsNotNearArMarkers_WhenUserMovesCloser_ArMarkersAreShown()
        {
            Repositories.MarkerRepository.Save(new[] {new Marker("north", 10, 0)});
            _markerBehaviour.Start();
            _markerBehaviour.ArCameraComponent.transform.position = new Vector3(0, 0, 0);
            _markerBehaviour.Update();
            _markerBehaviour.ArCameraComponent.transform.position = new Vector3(6, 0, 0);
            
            _markerBehaviour.Update();
            
            var northMarkerGameObject = _markerBehaviour.ArMarkers.First(marker => marker.name.Equals("north"));
            
            Assert.True(northMarkerGameObject.activeSelf);
        }
    }
}