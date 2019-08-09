using System.Collections;
using Locations;
using Markers;
using NUnit.Framework;
using SyncPoints;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayMode.Markers
{
    public class MarkerBehaviourTests
    {
        private static IEnumerator LoadScene()
        {
            Repositories.LocationsRepository.Save(new []{new Location("", "ChicagoMap") });
            Repositories.SyncPointRepository.Save(new []{new SyncPoint("test", 10, 10, 0), });
            
            SceneManager.LoadScene("ARView");
            yield return null;
            GameObject.Find("Overlay Map").GetComponent<InitializeMarkers>();
        }

        [UnityTest]
        public IEnumerator Start_WillLoadMarkerOntoView()
        {
            var testMarker = new Marker("Test Marker", 0, 0);

            Repositories.MarkerRepository.Save(new[] {testMarker});

            yield return LoadScene();

            AssertGameObjectCreatedCorrectly(testMarker);
        }

        [UnityTest]
        public IEnumerator Start_WillLoadMarkersFromRepoOntoView()
        {
            var testMarker1 = new Marker("Marker 1", 1, 2);
            var testMarker2 = new Marker("Marker 2", 10, 20);
            var markers = new[] {testMarker1, testMarker2};

            Repositories.MarkerRepository.Save(markers);

            yield return LoadScene();

            AssertGameObjectCreatedCorrectly(testMarker1);
            AssertGameObjectCreatedCorrectly(testMarker2);
        }

        private static void AssertGameObjectCreatedCorrectly(Marker testMarker)
        {
            var marker = GameObject.Find(testMarker.Label);
            var text = marker.GetComponentInChildren<Text>().text;
            var position = marker.transform.position;
        
            Assert.AreEqual(testMarker.Label, text);
            Assert.AreEqual(testMarker.X, position.x);
            Assert.AreEqual(testMarker.Z, position.z);
        }
    }
}