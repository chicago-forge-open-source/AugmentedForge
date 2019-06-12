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
        public void Update_WillOrientTheCameraRotationBasedOnTheCompass_BasedOnFractionOfRotation_NorthStartPosition()
        {
            var divisor = 4;

            _mapScript.arCamera = _game.AddComponent<Camera>();
            _mapScript.debugText = _game.AddComponent<Text>();
            _mapScript.startPoint = new GameObject();

            _mapScript.compass = new MockCompass {TrueHeading = 270f};
            _camera.transform.rotation = Quaternion.Euler(0, 0, 0);
            _mapScript.mapCamera = _camera;

            _mapScript.arCamera.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var expectedCameraRotation = Quaternion.Euler(0, 0, (360 - _mapScript.compass.TrueHeading) / divisor);
            Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_comparer));
        }

        [Test]
        public void Update_WillOrientCameraRotationBasedOnTheCompass_BasedOnFractionOfRotation_EastStartPosition()
        {
            var divisor = 4;

            _mapScript.arCamera = _game.AddComponent<Camera>();
            _mapScript.debugText = _game.AddComponent<Text>();
            _mapScript.startPoint = new GameObject();

            _mapScript.compass = new MockCompass {TrueHeading = 180f};
            var originalCameraRotationDegrees = 90;
            var originalCameraRotation = Quaternion.Euler(0, 0, originalCameraRotationDegrees);
            _camera.transform.rotation = originalCameraRotation;
            _mapScript.mapCamera = _camera;

            _mapScript.arCamera.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var differenceInRotation = (360 - _mapScript.compass.TrueHeading) - originalCameraRotationDegrees;
            var expectedCameraRotation = Quaternion.Euler(
                0,
                0,
                originalCameraRotationDegrees + differenceInRotation / divisor
            );

            Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_comparer));
        }

        [Test]
        public void Update_WillOrientCameraRotationBasedOnTheCompass_BasedOnFractionOfRotation_ChangesNearNorth()
        {
            var divisor = 4;

            _mapScript.arCamera = _game.AddComponent<Camera>();
            _mapScript.debugText = _game.AddComponent<Text>();
            _mapScript.startPoint = new GameObject();

            _mapScript.compass = new MockCompass {TrueHeading = 2f};
            var originalCameraRotationDegrees = 2;
            var originalCameraRotation = Quaternion.Euler(0, 0, originalCameraRotationDegrees);
            _camera.transform.rotation = originalCameraRotation;
            _mapScript.mapCamera = _camera;

            _mapScript.arCamera.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var differenceInRotation = -4;
            var expectedCameraRotation = Quaternion.Euler(
                0,
                0,
                originalCameraRotationDegrees + differenceInRotation / divisor
            );

            Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_comparer));
        }

        [Test]
        public void
            Update_WillOrientCameraRotationBasedOnTheCompass_BasedOnFractionOfRotation_ChangesNearNorth_OtherDirection()
        {
            var divisor = 4;

            _mapScript.arCamera = _game.AddComponent<Camera>();
            _mapScript.debugText = _game.AddComponent<Text>();
            _mapScript.startPoint = new GameObject();

            _mapScript.compass = new MockCompass {TrueHeading = 358f};
            var originalCameraRotationDegrees = 358;
            var originalCameraRotation = Quaternion.Euler(0, 0, originalCameraRotationDegrees);
            _camera.transform.rotation = originalCameraRotation;
            _mapScript.mapCamera = _camera;

            _mapScript.arCamera.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var differenceInRotation = 4;
            var expectedCameraRotation = Quaternion.Euler(
                0,
                0,
                originalCameraRotationDegrees + differenceInRotation / divisor
            );

            Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_comparer));
        }

        [Test]
        public void Start_WillUpdateArSessionOriginRotationBasedOnCompass()
        {
            _mapScript.arCamera = _game.AddComponent<Camera>();
            _mapScript.debugText = _game.AddComponent<Text>();
            _mapScript.startPoint = new GameObject();

            _mapScript.compass = new MockCompass {TrueHeading = 180f};

            _mapScript.arSessionOrigin = new GameObject();

            _mapScript.Start();

            var expectedCameraRotation = Quaternion.Euler(0, 180f, 0);

            Assert.That(_mapScript.arSessionOrigin.transform.rotation,
                Is.EqualTo(expectedCameraRotation).Using(_comparer)
            );
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