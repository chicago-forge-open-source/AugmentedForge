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
            GameObject map = GameObject.Find("Overlay Map");
            var overlayMapInitialize = map.GetComponent<OverlayMapInitialize>();
            overlayMapInitialize.Start();
        }

        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            yield return null;
        }
    }
}
