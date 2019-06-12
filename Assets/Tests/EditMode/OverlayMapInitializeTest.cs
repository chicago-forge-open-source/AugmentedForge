using Assets.Scripts;
using AugmentedForge;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using UnityEngine.UI;

namespace Assets.Tests.EditMode
{
    public class OverlayMapInitializeTest
    {
        private const int MapRotationIncrementDivisor = 4;

        private GameObject _game;
        private readonly QuaternionEqualityComparer _comparer = new QuaternionEqualityComparer(10e-6f);
        private readonly Camera _camera = Camera.main;
        private OverlayMapInitialize _mapScript;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _mapScript = _game.AddComponent<OverlayMapInitialize>();
            _mapScript.MapCamera = _camera;
            _mapScript.LocationMarker = new GameObject();
            _mapScript.StartPoint = new GameObject();
            _mapScript.ArSessionOrigin = new GameObject();
            _mapScript.ArCamera = _game.AddComponent<Camera>();
            _mapScript.DebugText = _game.AddComponent<Text>();
        }

        [Test]
        public void Start_WillMoveLocationMarkerToStartPoint()
        {
            var startPosition = new Vector3(3, 6, 9);
            _mapScript.StartPoint.transform.position = startPosition;

            _mapScript.Start();

            var position = _mapScript.LocationMarker.transform.position;
            var expectedVector = new Vector3(startPosition.x, startPosition.y, 0);
            Assert.AreEqual(expectedVector, position);
        }

        [Test]
        public void Start_WillMoveMapCameraToStartPoint()
        {
            var startPosition = new Vector3(1, 2, 3);
            _mapScript.StartPoint.transform.position = startPosition;

            _mapScript.Start();

            var position = _camera.transform.position;
            Assert.AreEqual(startPosition.x, position.x);
            Assert.AreEqual(startPosition.y, position.y);
        }

        [Test]
        public void Start_WillUpdateArSessionOriginRotationBasedOnCompass()
        {
            _mapScript.Compass = new MockCompass {TrueHeading = 180f};

            _mapScript.Start();

            var expectedCameraRotation = Quaternion.Euler(0, 180f, 0);

            Assert.That(_mapScript.ArSessionOrigin.transform.rotation,
                Is.EqualTo(expectedCameraRotation).Using(_comparer)
            );
        }

        [Test]
        public void Update_WillMoveLocationMarkerBasedOnArCameraLocation()
        {
            _mapScript.ArCamera.transform.position = new Vector3(15, 90, 34);
            _mapScript.StartPoint.transform.position = new Vector3(100, 20);
            _mapScript.Update();

            Assert.AreEqual(new Vector3(115, 54), _mapScript.LocationMarker.transform.position);
        }

        [Test]
        public void Update_GivenCompassWillRotateTheMapCameraIncrementally_NorthStartPosition()
        {
            _mapScript.Compass = new MockCompass {TrueHeading = 270f};
            _camera.transform.rotation = Quaternion.Euler(0, 0, 0);
            _mapScript.MapCamera = _camera;

            _mapScript.ArCamera.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var expectedCameraRotation =
                Quaternion.Euler(0, 0, (360 - _mapScript.Compass.TrueHeading) / MapRotationIncrementDivisor);
            Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_comparer));
        }

        [Test]
        public void Update_GivenCompassWillRotateTheMapCameraIncrementally_EastStartPosition()
        {
            _mapScript.Compass = new MockCompass {TrueHeading = 180f};
            var originalCameraRotationDegrees = 90;
            var originalCameraRotation = Quaternion.Euler(0, 0, originalCameraRotationDegrees);
            _camera.transform.rotation = originalCameraRotation;
            _mapScript.MapCamera = _camera;

            _mapScript.ArCamera.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var differenceInRotation = (360 - _mapScript.Compass.TrueHeading) - originalCameraRotationDegrees;
            var expectedCameraRotation = Quaternion.Euler(
                0,
                0,
                originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor
            );

            Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_comparer));
        }

        [Test]
        public void Update_GivenCompassWillRotateTheMapCameraIncrementally_ChangesNearNorth()
        {
            _mapScript.Compass = new MockCompass {TrueHeading = 2f};
            var originalCameraRotationDegrees = 2;
            var originalCameraRotation = Quaternion.Euler(0, 0, originalCameraRotationDegrees);
            _camera.transform.rotation = originalCameraRotation;
            _mapScript.MapCamera = _camera;

            _mapScript.ArCamera.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var differenceInRotation = -4;
            var expectedCameraRotation = Quaternion.Euler(
                0,
                0,
                originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor
            );

            Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_comparer));
        }

        [Test]
        public void Update_GivenCompassWillRotateTheMapCameraIncrementally_ChangesNearNorth_OtherDirection()
        {
            _mapScript.Compass = new MockCompass {TrueHeading = 358f};
            var originalCameraRotationDegrees = 358;
            var originalCameraRotation = Quaternion.Euler(0, 0, originalCameraRotationDegrees);
            _camera.transform.rotation = originalCameraRotation;
            _mapScript.MapCamera = _camera;

            _mapScript.ArCamera.transform.position = new Vector3(15, 35, 34);
            _mapScript.Update();

            var differenceInRotation = 4;
            var expectedCameraRotation = Quaternion.Euler(
                0,
                0,
                originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor
            );

            Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_comparer));
        }
    }

    internal class MockCompass : ICompass
    {
        public bool IsEnabled => true;
        public float TrueHeading { get; set; } = 100f;
    }
}