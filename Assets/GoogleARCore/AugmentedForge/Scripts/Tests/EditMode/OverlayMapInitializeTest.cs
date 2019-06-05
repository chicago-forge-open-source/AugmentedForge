using GoogleARCore.AugmentedForge.Scripts.Main;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace GoogleARCore.AugmentedForge.Scripts.Tests.EditMode
{
    public class OverlayMapInitializeTest
    {
        private readonly QuaternionEqualityComparer _comparer = new QuaternionEqualityComparer(10e-6f);
        private readonly Camera _camera = Camera.main;
        private readonly GameObject _locationMarker = new GameObject();
        private OverlayMapInitialize _mapScript;

        [SetUp]
        public void Setup()
        {
            _mapScript = new GameObject().AddComponent<OverlayMapInitialize>();
            _mapScript.mainCamera = _camera;
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
            Assert.AreEqual(vector, position);
        }

        [Test]
        public void GivenLocationSyncCameraIsMovedToViewLocation()
        {
            var vector = new Vector3(1,2,3);
            var syncPoint = new GameObject();
            syncPoint.transform.position = vector;

            _mapScript.LocationSync(syncPoint);
            var position = _camera.transform.position;
            Assert.AreEqual(vector.x, position.x);
            Assert.AreEqual(vector.y, position.y);
        }

        [Test]
        public void GivenChangeInPhysicalLocationMoveLocationTracker()
        {
            
        }
        
    }

    internal class NoCompass : ICompassInterface
    {
        public bool IsEnabled => false;
        public float TrueHeading => 0f;
    }

    internal class MockCompass : ICompassInterface
    {
        public bool IsEnabled => true;
        public float TrueHeading => 100f;
    }
}