using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Marker;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Assets.Tests.PlayMode.Marker
{
    public class MarkerBehaviourTests
    {
        private static IEnumerator LoadScene()
        {
            SceneManager.LoadScene("ARView");
            yield return null;
            GameObject.Find("Overlay Map").GetComponent<MarkerBehaviour>();
        }

        [UnityTest]
        public IEnumerator Start_WillLoadMarkerOntoView()
        {
            var testMarker = new global::Assets.Scripts.Marker.Marker("Test Marker", 0, 0);

            Repositories.MarkerRepository.Save(new[] {testMarker});

            yield return LoadScene();

            AssertGameObjectCreatedCorrectly(testMarker);
        }

        [UnityTest]
        public IEnumerator Start_WillLoadMarkersFromRepoOntoView()
        {
            var testMarker1 = new global::Assets.Scripts.Marker.Marker("Marker 1", 1, 2);
            var testMarker2 = new global::Assets.Scripts.Marker.Marker("Marker 2", 10, 20);
            var markers = new[] {testMarker1, testMarker2};

            Repositories.MarkerRepository.Save(markers);

            yield return LoadScene();

            AssertGameObjectCreatedCorrectly(testMarker1);
            AssertGameObjectCreatedCorrectly(testMarker2);
        }

        private static void AssertGameObjectCreatedCorrectly(global::Assets.Scripts.Marker.Marker testMarker)
        {
            var marker = GameObject.Find(testMarker.label);
            var text = marker.GetComponentInChildren<Text>().text;
            var position = marker.transform.position;
        
            Assert.AreEqual(testMarker.label, text);
            Assert.AreEqual(testMarker.x, position.x);
            Assert.AreEqual(testMarker.z, position.z);
        }
    }
}