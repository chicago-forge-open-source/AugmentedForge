using System.Collections;
using System.IO;
using Graffiti;
using Locations;
using NUnit.Framework;
using SyncPoints;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayMode.Graffiti
{
    public class GraffitiWallTests
    {
        private static IEnumerator LoadScene()
        {
            Repositories.LocationsRepository.Save(new []{new Location("", "ChicagoMap") });
            Repositories.SyncPointRepository.Save(new []{new SyncPoint("test", 10, 10, 0), });

            SceneManager.LoadScene("ARView");
            yield return null;
        }

        [UnityTest]
        public IEnumerator ClickingSaveOnGraffitiWallWillSave()
        {
            yield return LoadScene();
            GameObject.Find("GraffitiWall").GetComponent<GraffitiWallBehaviour>().dropGraffitiUi.SetActive(true);

            File.Delete($"{Application.persistentDataPath}/SavedImage.csv");

            GameObject.Find("Ok Button").GetComponent<Button>().onClick.Invoke();

            Assert.IsTrue(File.Exists($"{Application.persistentDataPath}/SavedImage.csv"));
        }

        [UnityTest]
        public IEnumerator SelectedOKButtonDoesNotAllowDropPointToBeUpdated()
        {
            yield return LoadScene();
            var wallBehaviour = GameObject.Find("GraffitiWall").GetComponent<GraffitiWallBehaviour>();
            var wallInputBehavior = wallBehaviour.dropGraffitiInputBehaviour;
            wallInputBehavior.planeTouchDetector 
                = new MockPlaneTouchDetector(new Vector2(0, 0));

            wallBehaviour.dropGraffitiUi.SetActive(true);
            Vector2 dropPointBefore = wallInputBehavior.dropPoint;
            GameObject.Find("Ok Button").GetComponent<Button>().Select();
            wallInputBehavior.Update();
            Vector2 dropPointAfter = wallInputBehavior.dropPoint;
            
            Assert.IsTrue(dropPointBefore.Equals(dropPointAfter));
        }
    }
    
    public class MockPlaneTouchDetector : PlaneTouchDetector
    {
        public Transform lastTransform;
        public Camera lastCamera;
        public int lastTextureSize;

        public MockPlaneTouchDetector(Vector2 touchToReturn)
        {
            PointToReturn = touchToReturn;
        }

        public Vector2? FindTouchedPoint(Transform transform, Camera camera, int textureSize)
        {
            lastTextureSize = textureSize;
            lastCamera = camera;
            lastTransform = transform;
            return PointToReturn;
        }

        public Vector2? PointToReturn { get; set; }
    }
}