using Main;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Tests.EditMode
{
    public class OverlayMapInitializeTest
    {
        private readonly QuaternionEqualityComparer _comparer = new QuaternionEqualityComparer(10e-6f);
        private readonly GameObject _map = new GameObject("Overlay Map");
        private readonly Camera _camera = Camera.main;
        private OverlayMapInitialize _mapScript;

        [SetUp]
        public void Setup()
        {
            _mapScript = _map.AddComponent<OverlayMapInitialize>();
            _mapScript.mainCamera = _camera;
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
            var locationMarker = new GameObject();
            _mapScript.locationMarker = locationMarker;

            var vector = new Vector3(3, 6, 9);
            var syncPoint = new GameObject();
            syncPoint.transform.position = vector;

            _mapScript.LocationSync(syncPoint);
            var position = locationMarker.transform.position;
            Assert.AreEqual(vector, position);
        }

        [Test]
        public void GivenLocationSyncCameraIsMovedToViewLocation()
        {
            var locationMarker = new GameObject();
            _mapScript.locationMarker = locationMarker;

            var vector = new Vector3(1,2,3);
            var syncPoint = new GameObject();
            syncPoint.transform.position = vector;

            _mapScript.LocationSync(syncPoint);
            var position = _camera.transform.position;
            Assert.AreEqual(vector.x, position.x);
            Assert.AreEqual(vector.y, position.y);
        }
    }

    internal class NoCompass : CompassInterface
    {
        public bool IsEnabled => false;
        public float TrueHeading => 0f;
    }

    internal class MockCompass : CompassInterface
    {
        public bool IsEnabled => true;
        public float TrueHeading => 100f;
    }
}