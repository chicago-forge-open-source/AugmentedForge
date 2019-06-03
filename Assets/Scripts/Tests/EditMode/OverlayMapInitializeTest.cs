using System.Collections;
using Main;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.EditMode
{
    public class OverlayMapInitializeTest
    {
        [Test]
        public void WhenNoCompassDetectedMapIsRotatedToZero()
        {
            var map = GameObject.Find("Overlay Map");
            var overlayMapInitialize = map.GetComponent<OverlayMapInitialize>();
            overlayMapInitialize.AlignMapWithCompass(null);
            var defaultQuaternion = new Quaternion(0.0f, 0.0f, 0.0f, 1f);
            Assert.AreEqual(defaultQuaternion, map.transform.rotation);
        }

        [Test]
        public void WhenCompassDetectedMapIsRotatedToMatchNorth()
        {
            var map = GameObject.Find("Overlay Map");
            var overlayMapInitialize = map.GetComponent<OverlayMapInitialize>();
            var mockCompass = new MockCompass();
            overlayMapInitialize.AlignMapWithCompass(mockCompass);
            var compassQuaternion = Quaternion.Euler(0, -mockCompass.TrueHeading, 0);
            Assert.AreEqual(compassQuaternion,map.transform.rotation);
        }
    }

    public class MockCompass : CompassInterface
    {
        public float TrueHeading => 100f;
    }
}