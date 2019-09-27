using System.Collections;
using System.IO;
using Locations;
using Markers;
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
            GameObject.Find("SketcherUI").GetComponent<Canvas>().enabled = true;

            File.Delete($"{Application.persistentDataPath}/SavedImage.csv");

            GameObject.Find("Save Button").GetComponent<Button>().onClick.Invoke();

            Assert.IsTrue(File.Exists($"{Application.persistentDataPath}/SavedImage.csv"));
        }
    }
}