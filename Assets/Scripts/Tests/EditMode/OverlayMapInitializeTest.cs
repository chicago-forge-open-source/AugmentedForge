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
            overlayMapInitialize.Start();
            var defaultQuaternion = new Quaternion(0.0f, 0.0f, 0.0f, 1f);
            Assert.AreEqual(defaultQuaternion, map.transform.rotation);
        }
    }
}