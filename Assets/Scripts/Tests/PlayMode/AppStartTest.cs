using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Tests.PlayMode
{
    public class AppStartTest
    {
        [UnityTest]
        [UnityPlatform(RuntimePlatform.Android)]
        public IEnumerator WhenSceneIsLoadedAndCompassEnabledOverlayMapIsRotated()
        {
            SceneManager.LoadScene("SampleScene");
            yield return null;

            var map = GameObject.Find("Overlay Map");
            var defaultQuaternion = Quaternion.Euler(0, 0, 0);
            var comparer = new QuaternionEqualityComparer(10e-6f);

            yield return new WaitUntil(() => Math.Abs(Input.compass.trueHeading) > 0);

            Assert.That(map.transform.rotation, Is.Not.EqualTo(defaultQuaternion).Using(comparer));
        }
    }
}
