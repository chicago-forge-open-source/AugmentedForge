using Main;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Tests.EditMode
{
    public class OverlayMapInitializeTest
    {
        private readonly GameObject _map = new GameObject("Overlay Map");
        private readonly QuaternionEqualityComparer _comparer = new QuaternionEqualityComparer(10e-6f);
        private OverlayMapInitialize _mapScript;

        [SetUp]
        public void Setup()
        {
            _mapScript = _map.AddComponent<OverlayMapInitialize>();
        }

        [Test]
        public void WhenNoCompassDetectedMapIsRotatedToZero()
        {
            _mapScript.AlignMapWithCompass(new NoCompass());
            var defaultQuaternion = Quaternion.Euler(0, 0, 0);
            Assert.That(_map.transform.rotation, Is.EqualTo(defaultQuaternion).Using(_comparer));
        }

        [Test]
        public void WhenCompassDetectedMapIsRotatedToMatchNorth()
        {
            var mockCompass = new MockCompass();
            _mapScript.AlignMapWithCompass(mockCompass);
            var compassQuaternion = Quaternion.Euler(0, -mockCompass.TrueHeading, 0);
            Assert.That(_map.transform.rotation, Is.EqualTo(compassQuaternion).Using(_comparer));
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