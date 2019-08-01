using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Markers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using UnityEngine.XR.ARFoundation;

namespace Assets.Tests.EditMode
{
    public class OverlayMapBehaviourEditTests
    {
        private const int MapRotationIncrementDivisor = 4;
        private GameObject _game;
        private readonly QuaternionEqualityComparer _quaternionComparer = new QuaternionEqualityComparer(10e-6f);
        private OverlayMapBehaviour _mapScript;
        private const string Chicago = "Chicago";

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _game.AddComponent<SpriteRenderer>();
            _mapScript = _game.AddComponent<OverlayMapBehaviour>();

            _mapScript.MapCameraComponent = new GameObject();
            _mapScript.MapCameraComponent.AddComponent<Camera>();
            _mapScript.MapCameraComponent.AddComponent<FingerGestures>();

            _mapScript.ArCameraComponent = new GameObject();
            _mapScript.ArCameraComponent.AddComponent<ARCameraBackground>();
            _mapScript.ArCameraComponent.GetComponent<Camera>().cullingMask = 567;

            _mapScript.LocationMarker = new GameObject();
            _mapScript.StartPoint = new GameObject();

            _mapScript.InitializeMarkers = _game.AddComponent<InitializeMarkers>();

            PlayerPrefs.SetString("location", Chicago);
        }

        [Test]
        public void Start_WillMoveLocationMarkerToStartPoint()
        {
            _mapScript.Start();

            var position = _mapScript.LocationMarker.transform.position;
            
            Assert.AreEqual(_mapScript.StartPoint.transform.position, position);
        }

        [Test]
        public void Start_WillMoveMapCameraToStartPoint()
        {
            _mapScript.Start();

            var position = _mapScript.MapCameraComponent.transform.position;
            var startPosition = _mapScript.StartPoint.transform.position;
            Assert.AreEqual(startPosition.x, position.x);
            Assert.AreEqual(startPosition.z, position.z);
        }

        [Test]
        public void Start_WillLoadCorrectMapSpriteBasedOnLocationSelected()
        {
            _mapScript.Start();

            var spriteName = _mapScript.GetComponent<SpriteRenderer>().sprite.name;
            Assert.AreEqual(Chicago + "MapSprite", spriteName);
        }

        [Test]
        public void Start_WillLoadCorrectMapSpriteOfDifferentLocation()
        {
            const string location = "Iowa";
            PlayerPrefs.SetString("location", location);

            _mapScript.Start();

            var spriteName = _mapScript.GetComponent<SpriteRenderer>().sprite.name;
            Assert.AreEqual(location + "MapSprite", spriteName);
        }

        [Test]
        public void Update_WillMoveLocationMarkerToArCameraLocation()
        {
            _mapScript.Start();

            _mapScript.ArCameraComponent.transform.position = new Vector3(15, 90, 34);
            _mapScript.StartPoint.transform.position = new Vector3(100, 13, 20);
            _mapScript.Update();

            Assert.AreEqual(new Vector3(15, 13, 34), _mapScript.LocationMarker.transform.position);
        }

        [Test]
        public void Update_RotationWillNotChangeWhenARCameraBackgroundNotEnabled()
        {
            StartInMapOnlyMode();

            _mapScript.Compass = new MockCompass {TrueHeading = 90f};
            _mapScript.MapCameraComponent.transform.rotation = Quaternion.Euler(90, 0, 0);

            _mapScript.ArCameraComponent.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var expectedCameraRotation = Quaternion.Euler(90, 0, 0);
            Assert.That(_mapScript.MapCameraComponent.transform.rotation,
                Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
        }

        [Test]
        public void Update_GivenCompassWillRotateTheMapCameraIncrementally_NorthStartPosition()
        {
            _mapScript.Start();

            _mapScript.Compass = new MockCompass {TrueHeading = 90f};
            _mapScript.MapCameraComponent.transform.rotation = Quaternion.Euler(90, 0, 0);

            _mapScript.ArCameraComponent.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var expectedCameraRotation = Quaternion.Euler(
                90,
                _mapScript.Compass.TrueHeading / MapRotationIncrementDivisor,
                0);
            Assert.That(_mapScript.MapCameraComponent.transform.rotation,
                Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
        }

        [Test]
        public void Update_GivenCompassWillRotateTheMapCameraIncrementally_EastStartPosition()
        {
            _mapScript.Start();

            _mapScript.Compass = new MockCompass {TrueHeading = 180f};
            var originalCameraRotationDegrees = 90;
            var originalCameraRotation = Quaternion.Euler(90, originalCameraRotationDegrees, 0);
            _mapScript.MapCameraComponent.transform.rotation = originalCameraRotation;

            _mapScript.ArCameraComponent.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var differenceInRotation = _mapScript.Compass.TrueHeading - originalCameraRotationDegrees;
            var expectedCameraRotation = Quaternion.Euler(
                90,
                originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor,
                0
            );

            Assert.That(_mapScript.MapCameraComponent.transform.rotation,
                Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
        }

        [Test]
        public void Update_GivenCompassWillRotateTheMapCameraIncrementally_ChangesNearNorth()
        {
            _mapScript.Start();

            _mapScript.Compass = new MockCompass {TrueHeading = 358f};
            const int originalCameraRotationDegrees = 2;
            var originalCameraRotation = Quaternion.Euler(90, originalCameraRotationDegrees, 0);
            _mapScript.MapCameraComponent.transform.rotation = originalCameraRotation;

            _mapScript.ArCameraComponent.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var differenceInRotation = -4;
            var expectedCameraRotation = Quaternion.Euler(
                90,
                originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor,
                0
            );

            Assert.That(_mapScript.MapCameraComponent.transform.rotation,
                Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
        }

        [Test]
        public void Update_GivenCompassWillRotateTheMapCameraIncrementally_ChangesNearNorth_OtherDirection()
        {
            _mapScript.Start();

            _mapScript.Compass = new MockCompass {TrueHeading = 2f};
            var originalCameraRotationDegrees = 358;
            var originalCameraRotation = Quaternion.Euler(90, originalCameraRotationDegrees, 0);
            _mapScript.MapCameraComponent.transform.rotation = originalCameraRotation;

            _mapScript.ArCameraComponent.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var differenceInRotation = 4;
            var expectedCameraRotation = Quaternion.Euler(
                90,
                originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor,
                0
            );

            Assert.That(_mapScript.MapCameraComponent.transform.rotation,
                Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
        }

        [Test]
        public void Update_WillRotateOverlayMarkersInTheOppositeDirectionOfTheMapSoTheyRemainReadable()
        {
            _mapScript.InitializeMarkers.MapMarkers.Add(new GameObject("north"));
            _mapScript.Start();

            _mapScript.Compass = new MockCompass {TrueHeading = 2f};
            var originalCameraRotationDegrees = 358;
            var originalCameraRotation = Quaternion.Euler(90, originalCameraRotationDegrees, 0);
            _mapScript.MapCameraComponent.transform.rotation = originalCameraRotation;
            
            _mapScript.Update();

            var differenceInRotation = 4;
            var expectedCameraRotation = Quaternion.Euler(
                90,
                originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor,
                0
            );

            Assert.That(_mapScript.InitializeMarkers.MapMarkers.First(marker => marker.name.Equals("north")).transform.rotation,
                Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
        }

        [Test]
        public void Update_GivenArCameraBackgroundIsDisabled_MapCameraWillNotChange()
        {
            StartInMapOnlyMode();

            var arCameraPos = new Vector3(5, 1, 5);
            _mapScript.ArCameraComponent.transform.position = arCameraPos;

            var mapCameraPos = new Vector3(10, 5, 10);
            _mapScript.MapCameraComponent.transform.position = mapCameraPos;

            _mapScript.Update();

            var position = _mapScript.MapCameraComponent.transform.position;
            Assert.AreEqual(10, position.x);
            Assert.AreEqual(5, position.y);
            Assert.AreEqual(10, position.z);
        }

        [Test]
        public void Update_GivenChangeInUserLocationMoveMapCameraToSameLocation()
        {
            _mapScript.Start();

            var arCameraPos = new Vector3(5, 1, 5);
            _mapScript.ArCameraComponent.transform.position = arCameraPos;

            var mapCameraPos = new Vector3(10, 5, 10);
            _mapScript.MapCameraComponent.transform.position = mapCameraPos;

            _mapScript.Update();

            var arCameraPosition = _mapScript.ArCameraComponent.transform.position;
            var position = _mapScript.MapCameraComponent.transform.position;

            Assert.AreEqual(arCameraPosition.x, position.x);
            Assert.AreEqual(mapCameraPos.y, position.y);
            Assert.AreEqual(arCameraPosition.z, position.z);
        }

        private void StartInMapOnlyMode()
        {
            _mapScript.Start();
            _mapScript.ArCameraComponent.GetComponent<ARCameraBackground>().enabled = false;
        }

        [Test]
        public void Start_GivenChicagoAsTheLocation_ChicagoSyncPointIsLoaded()
        {
            _mapScript.Start();
            
            Assert.AreEqual("Chicago Sync Point", _mapScript.StartPoint.name);
        }
        
        [Test]
        public void Start_GivenIowaAsTheLocation_IowaSyncPointIsLoaded()
        {
            PlayerPrefs.SetString("location", "Iowa");
            
            _mapScript.Start();
            
            Assert.AreEqual("Iowa Sync Point", _mapScript.StartPoint.name);
        }
    }

    internal class MockCompass : ICompass
    {
        public bool IsEnabled => true;
        public float TrueHeading { get; set; } = 100f;
    }
}