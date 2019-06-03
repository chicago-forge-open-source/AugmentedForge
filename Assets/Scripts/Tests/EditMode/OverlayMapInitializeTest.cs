using System.Collections.Generic;
using Main;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests.EditMode
{
    public class OverlayMapInitializeTest
    {
        private readonly GameObject _map = new GameObject("Overlay Map");
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
            Assert.AreEqual(defaultQuaternion, _map.transform.rotation);
        }

        [Test]
        public void WhenCompassDetectedMapIsRotatedToMatchNorth()
        {
            var mockCompass = new MockCompass();
            _mapScript.AlignMapWithCompass(mockCompass);
            var compassQuaternion = Quaternion.Euler(0, -mockCompass.TrueHeading, 0);
            Assert.AreEqual(compassQuaternion, _map.transform.rotation);
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