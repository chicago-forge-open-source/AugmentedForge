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
        private readonly QuaternionEqualityComparer _comparer = new QuaternionEqualityComparer(10e-6f);

        [UnityTest]
        [UnityPlatform(RuntimePlatform.Android)]
        public IEnumerator WhenCompassEnabledCameraIsRotated()
        {
            SceneManager.LoadScene("MapScene");
            yield return null;

            var camera = GameObject.Find("Main Camera");
            var defaultQuaternion = Quaternion.Euler(0, 0, 0);

            yield return new WaitUntil(() => Math.Abs(Input.compass.trueHeading) > 0);

            Assert.That(camera.transform.rotation, Is.Not.EqualTo(defaultQuaternion).Using(_comparer));
        }

        [UnityTest]
        [UnityPlatform(RuntimePlatform.Android)]
        public IEnumerator OnStartLocationMarkerIsSetToSyncPoint()
        {
            SceneManager.LoadScene("MapScene");
            yield return null;

            var locationMarker = GameObject.Find("Location Marker");
            var syncPoint = GameObject.Find("Sync Point 1");

            Assert.AreEqual(syncPoint.transform.position, locationMarker.transform.position);
        }
    }
}