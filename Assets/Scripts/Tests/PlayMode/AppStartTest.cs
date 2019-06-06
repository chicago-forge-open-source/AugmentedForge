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
        private GameObject _camera;
        private GameObject _syncPoint;

        private IEnumerator LoadScene()
        {
            SceneManager.LoadScene("MapScene");
            yield return null;
            _camera = GameObject.Find("Main Camera");
            _syncPoint = GameObject.Find("Sync Point 1");
        }

        [UnityTest]
        [UnityPlatform(RuntimePlatform.Android)]
        public IEnumerator WhenCompassEnabledCameraIsRotated()
        {
            yield return LoadScene();
            var comparer = new QuaternionEqualityComparer(10e-6f);
            var defaultQuaternion = Quaternion.Euler(0, 0, 0);
            yield return new WaitUntil(() => Math.Abs(Input.compass.trueHeading) > 0);
            Assert.That(_camera.transform.rotation, Is.Not.EqualTo(defaultQuaternion).Using(comparer));
        }

        [UnityTest]
        [UnityPlatform(RuntimePlatform.Android)]
        public IEnumerator OnStartLocationMarkerIsSetToSyncPoint()
        {
            yield return LoadScene();
            var locationMarker = GameObject.Find("Location Marker");

            Assert.AreEqual(_syncPoint.transform.position, locationMarker.transform.position);
        }

        [UnityTest]
        [UnityPlatform(RuntimePlatform.Android)]
        public IEnumerator OnStartCameraViewsLocationMarker()
        {
            yield return LoadScene();
            var position = _syncPoint.transform.position;

            var cameraPos = _camera.transform.position;
            Assert.AreEqual(position.x, cameraPos.x);
            Assert.AreEqual(position.y, cameraPos.y);
        }
    }
}