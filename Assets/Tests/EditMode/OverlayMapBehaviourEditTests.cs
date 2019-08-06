using System;
using DataLoaders;
using Markers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Tests.EditMode
{
    public class OverlayMapBehaviourEditTests
    {
        private GameObject _game;
        private OverlayMapBehaviour _mapScript;
        private const string Chicago = "Chicago";

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _game.AddComponent<SpriteRenderer>();
            _mapScript = _game.AddComponent<OverlayMapBehaviour>();

            _mapScript.mapCameraGameObject = new GameObject();
            _mapScript.mapCameraGameObject.AddComponent<Camera>();
            _mapScript.mapCameraGameObject.AddComponent<FingerGestures>();

            _mapScript.arCameraGameObject = new GameObject();
            _mapScript.arCameraGameObject.AddComponent<ARCameraBackground>();
            _mapScript.arCameraGameObject.GetComponent<Camera>().cullingMask = 567;

            _mapScript.locationMarker = new GameObject();
            _mapScript.startPoint = new GameObject();

            _mapScript.initializeMarkers = _game.AddComponent<InitializeMarkers>();

            PlayerPrefs.SetString("location", Chicago);
        }

        [Test]
        public void Start_WillMoveLocationMarkerToStartPoint()
        {
            _mapScript.Start();

            var position = _mapScript.locationMarker.transform.position;

            Assert.AreEqual(_mapScript.startPoint.transform.position, position);
        }

        [Test]
        public void Start_WillMoveMapCameraToStartPoint()
        {
            _mapScript.Start();

            var position = _mapScript.mapCameraGameObject.transform.position;
            var startPosition = _mapScript.startPoint.transform.position;
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
            TestHelpers.AssertQuaternionsAreEqual(
                Quaternion.Euler(90, 28, 0),
                _game.transform.rotation
            );
        }

        [Test]
        public void Update_WillMoveLocationMarkerToArCameraLocation()
        {
            _mapScript.Start();

            _mapScript.arCameraGameObject.transform.position = new Vector3(15, 90, 34);
            _mapScript.startPoint.transform.position = new Vector3(100, 13, 20);
            _mapScript.Update();

            Assert.AreEqual(new Vector3(15, 13, 34), _mapScript.locationMarker.transform.position);
        }

        [Test]
        public void Update_RotationWillNotChangeWhenARCameraBackgroundNotEnabled()
        {
            StartInMapOnlyMode();

            _mapScript.compass = new MockCompass {TrueHeading = 90f};
            _mapScript.mapCameraGameObject.transform.rotation = Quaternion.Euler(90, 0, 0);

            _mapScript.arCameraGameObject.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            TestHelpers.AssertQuaternionsAreEqual(
                Quaternion.Euler(90, 0, 0),
                _mapScript.mapCameraGameObject.transform.rotation
            );
        }

        [Test]
        public void Update_GivenCompassWillRotateTheMapCameraIncrementally_NorthStartPosition()
        {
            _mapScript.Start();

            _mapScript.compass = new MockCompass {TrueHeading = 90f};
            _mapScript.mapCameraGameObject.transform.rotation = Quaternion.Euler(90, 0, 0);

            _mapScript.arCameraGameObject.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            TestHelpers.AssertQuaternionsAreEqual(
                ExpectedCameraRotation(0, _mapScript.compass.TrueHeading),
                _mapScript.mapCameraGameObject.transform.rotation
            );
        }

        [Test]
        public void Update_GivenCompassWillRotateTheMapCameraIncrementally_EastStartPosition()
        {
            _mapScript.Start();

            _mapScript.compass = new MockCompass {TrueHeading = 180f};
            const int originalCameraDegrees = 90;
            var originalCameraRotation = Quaternion.Euler(90, originalCameraDegrees, 0);
            _mapScript.mapCameraGameObject.transform.rotation = originalCameraRotation;

            _mapScript.arCameraGameObject.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var rotationDiff = _mapScript.compass.TrueHeading - originalCameraDegrees;
            var expectedCameraRotation = ExpectedCameraRotation(originalCameraDegrees, rotationDiff);

            TestHelpers.AssertQuaternionsAreEqual(
                expectedCameraRotation,
                _mapScript.mapCameraGameObject.transform.rotation
            );
        }

        [Test]
        public void Update_GivenCompassWillRotateTheMapCameraIncrementally_ChangesNearNorth()
        {
            _mapScript.Start();

            _mapScript.compass = new MockCompass {TrueHeading = 358f};
            const int originalCameraDegrees = 2;
            var originalCameraRotation = Quaternion.Euler(90, originalCameraDegrees, 0);
            _mapScript.mapCameraGameObject.transform.rotation = originalCameraRotation;

            _mapScript.arCameraGameObject.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            TestHelpers.AssertQuaternionsAreEqual(
                ExpectedCameraRotation(originalCameraDegrees, -4),
                _mapScript.mapCameraGameObject.transform.rotation
            );
        }

        [Test]
        public void Update_GivenCompassWillRotateTheMapCameraIncrementally_ChangesNearNorth_OtherDirection()
        {
            _mapScript.Start();

            _mapScript.compass = new MockCompass {TrueHeading = 2f};
            const int originalCameraDegrees = 358;
            var originalCameraRotation = Quaternion.Euler(90, originalCameraDegrees, 0);
            _mapScript.mapCameraGameObject.transform.rotation = originalCameraRotation;

            _mapScript.arCameraGameObject.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            TestHelpers.AssertQuaternionsAreEqual(
                ExpectedCameraRotation(originalCameraDegrees, 4),
                _mapScript.mapCameraGameObject.transform.rotation
            );
        }

        [Test]
        public void Update_WillRotateOverlayMarkersInTheOppositeDirectionOfTheMapSoTheyRemainReadable()
        {
            _mapScript.initializeMarkers.MapMarkers.Add(new GameObject("north"));
            _mapScript.Start();

            _mapScript.compass = new MockCompass {TrueHeading = 2f};
            const int originalCameraDegrees = 358;
            var originalCameraRotation = Quaternion.Euler(90, originalCameraDegrees, 0);
            _mapScript.mapCameraGameObject.transform.rotation = originalCameraRotation;

            _mapScript.Update();

            TestHelpers.AssertQuaternionsAreEqual(
                ExpectedCameraRotation(originalCameraDegrees, 4),
                _mapScript.mapCameraGameObject.transform.rotation
            );
        }

        [Test]
        public void Update_GivenArCameraBackgroundIsDisabled_MapCameraWillNotChange()
        {
            StartInMapOnlyMode();

            var arCameraPos = new Vector3(5, 1, 5);
            _mapScript.arCameraGameObject.transform.position = arCameraPos;

            var mapCameraPos = new Vector3(10, 5, 10);
            _mapScript.mapCameraGameObject.transform.position = mapCameraPos;

            _mapScript.Update();

            var position = _mapScript.mapCameraGameObject.transform.position;
            Assert.AreEqual(10, position.x);
            Assert.AreEqual(5, position.y);
            Assert.AreEqual(10, position.z);
        }

        [Test]
        public void Update_GivenChangeInUserLocationMoveMapCameraToSameLocation()
        {
            _mapScript.Start();

            var arCameraPos = new Vector3(5, 1, 5);
            _mapScript.arCameraGameObject.transform.position = arCameraPos;

            var mapCameraPos = new Vector3(10, 5, 10);
            _mapScript.mapCameraGameObject.transform.position = mapCameraPos;

            _mapScript.Update();

            var arCameraPosition = _mapScript.arCameraGameObject.transform.position;
            var position = _mapScript.mapCameraGameObject.transform.position;

            Assert.AreEqual(arCameraPosition.x, position.x);
            Assert.AreEqual(mapCameraPos.y, position.y);
            Assert.AreEqual(arCameraPosition.z, position.z);
        }

        private void StartInMapOnlyMode()
        {
            _mapScript.Start();
            _mapScript.arCameraGameObject.GetComponent<ARCameraBackground>().enabled = false;
        }

        [Test]
        public void Start_GivenChicagoAsTheLocation_ChicagoSyncPointIsLoaded()
        {
            new ChicagoDataLoader().DataLoad();

            _mapScript.Start();

            var expectedSyncPointPosition = new Vector3(26.94955f, 0, -18.17933f);
            var actualSyncPointPosition = _mapScript.startPoint.transform.position;
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.x - actualSyncPointPosition.x) < .1);
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.y - actualSyncPointPosition.y) < .1);
        }

        [Test]
        public void Start_GivenIowaAsTheLocation_IowaSyncPointIsLoaded()
        {
            new IowaDataLoader().DataLoad();

            _mapScript.Start();

            var expectedSyncPointPosition = new Vector3(0, 0, -1.5f);
            var actualSyncPointPosition = _mapScript.startPoint.transform.position;
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.x - actualSyncPointPosition.x) < .01);
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.y - actualSyncPointPosition.y) < .01);
        }

        private static Quaternion ExpectedCameraRotation(float camRotation, float rotationDiff)
        {
            const int mapRotationIncrementDivisor = 4;

            return Quaternion.Euler(
                90,
                camRotation + rotationDiff / mapRotationIncrementDivisor,
                0
            );
        }
    }

    internal class MockCompass : ICompass
    {
        public bool IsEnabled => true;
        public float TrueHeading { get; set; } = 100f;
    }
}