using UnityEngine.UI;

namespace AugmentedForge.Tests
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class OverlayMapInitializeTest
    {
        private GameObject _game;
        private readonly QuaternionEqualityComparer _comparer = new QuaternionEqualityComparer(10e-6f);
        private readonly Camera _camera = Camera.main;
        private GameObject _locationMarker;
        private OverlayMapInitialize _mapScript;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _locationMarker = new GameObject();
            _mapScript = _game.AddComponent<OverlayMapInitialize>();
            _mapScript.mapCamera = _camera;
            _mapScript.locationMarker = _locationMarker;
        }

        [Test]
        public void WhenNoCompassDetectedCameraIsRotatedToZero()
        {
            _mapScript.AlignCameraWithCompass(new NoCompass());

            var defaultQuaternion = Quaternion.Euler(0, 0, 0);
            Assert.That(_camera.transform.rotation, Is.EqualTo(defaultQuaternion).Using(_comparer));
        }

        [Test]
        public void WhenCompassDetectedCameraIsRotatedToMatchNorth()
        {
            var mockCompass = new MockCompass();

            _mapScript.AlignCameraWithCompass(mockCompass);

            var compassQuaternion = Quaternion.Euler(0, 0, -mockCompass.TrueHeading);
            Assert.That(_camera.transform.rotation, Is.EqualTo(compassQuaternion).Using(_comparer));
        }

        [Test]
        public void GivenSyncPointLocationMarkerPositionIsSetToVector()
        {
            var vector = new Vector3(3, 6, 9);
            var syncPoint = new GameObject();
            syncPoint.transform.position = vector;

            _mapScript.LocationSync(syncPoint);

            var position = _locationMarker.transform.position;
            var expectedVector = new Vector3(vector.x, vector.y, 0);
            Assert.AreEqual(expectedVector, position);
        }

        [Test]
        public void GivenLocationSyncCameraIsMovedToViewLocation()
        {
            var vector = new Vector3(1, 2, 3);
            var syncPoint = new GameObject();
            syncPoint.transform.position = vector;

            _mapScript.LocationSync(syncPoint);

            var position = _camera.transform.position;
            Assert.AreEqual(vector.x, position.x);
            Assert.AreEqual(vector.y, position.y);
        }

        [Test]
        public void Update_WillChangeThePositionOfLocationMarkerBasedOnArCameraLocation()
        {
            var arCamera = _game.AddComponent<Camera>();
            _mapScript.debugText = _game.AddComponent<Text>();
            _mapScript.arCamera = arCamera;

            var startPoint = new GameObject();
            _mapScript.startPoint = startPoint;

            arCamera.transform.position = new Vector3(15, 90, 34);
            startPoint.transform.position = new Vector3(100, 20);
            _mapScript.Update();

            Assert.AreEqual(new Vector3(115, 54), _locationMarker.transform.position);
        }

        [Test]
        public void Update_WillOrientTheCameraRotationBasedOnTheCompass()
        {
            _mapScript.arCamera = _game.AddComponent<Camera>();
            _mapScript.debugText = _game.AddComponent<Text>();
            _mapScript.startPoint = new GameObject();

            _mapScript.compass = new MockCompass {TrueHeading = 30f};
            _mapScript.mapCamera = _camera;

            _mapScript.arCamera.transform.position = new Vector3(15, 90, 34);
            _mapScript.Update();

            var compassQuaternion = Quaternion.Euler(0, 0, -_mapScript.compass.TrueHeading);
            Assert.That(_camera.transform.rotation, Is.EqualTo(compassQuaternion).Using(_comparer));
        }
    }

    internal class NoCompass : ICompass
    {
        public bool IsEnabled => false;
        public float TrueHeading => 0f;
    }

    internal class MockCompass : ICompass
    {
        public bool IsEnabled => true;
        public float TrueHeading { get; set; } = 100f;
    }
}