using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class InitializeAppPlayTests
    {
        private InitializeApp _initScript;
    
        private IEnumerator SetupScene()
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("InitScene");
            yield return null;

            var camera = GameObject.Find("Main Camera");
            _initScript = camera.GetComponent<InitializeApp>();
            _initScript.compass = new MockCompass();
        }
    
        [UnityTest]
        public IEnumerator GivenChicagoForgeButtonClickLoadArViewForChicago()
        {
            yield return SetupScene();

            _initScript. OnClick_LoadLocationARView("Chicago");

            yield return new WaitForSeconds(0.1f);

            var location = PlayerPrefs.GetString("location");
        
            Assert.AreEqual("ARView", SceneManager.GetActiveScene().name);
            Assert.AreEqual("Chicago", location);
        }

        [UnityTest]
        public IEnumerator GivenIowaForgeButtonClickLoadArViewForIowa()
        {
            yield return SetupScene();

            _initScript.OnClick_LoadLocationARView("Iowa");

            yield return new WaitForSeconds(0.1f);
        
            var location = PlayerPrefs.GetString("location");

            Assert.AreEqual("ARView", SceneManager.GetActiveScene().name);
            Assert.AreEqual("Iowa", location);
        }

        [UnityTest]
        public IEnumerator GivenGrandOpeningForgeButtonClickLoadArViewForGrandOpening()
        {
            yield return SetupScene();
            //32, -9.23

            _initScript.OnClick_LoadLocationARView("GrandOpening");

            yield return new WaitForSeconds(0.1f);
        
            var location = PlayerPrefs.GetString("location");

            Assert.AreEqual("ARView", SceneManager.GetActiveScene().name);
            Assert.AreEqual("GrandOpening", location);
        }
    }
}