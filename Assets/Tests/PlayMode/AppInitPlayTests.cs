using System.Collections;
using AugmentedForge;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Assets.Tests.PlayMode
{
    public class AppInitPlayTests
    {
        private InitializeThings _initScript;
    
        private IEnumerator SetupScene()
        {
            SceneManager.LoadScene("InitScene");
            yield return null;

            var camera = GameObject.Find("Main Camera");
            _initScript = camera.GetComponent<InitializeThings>();
            _initScript.Compass = new MockCompass();
        }
    
        [UnityTest]
        public IEnumerator GivenChicagoForgeButtonClickLoadARViewForChicago()
        {
            yield return SetupScene();

            _initScript.OnClick_LoadLocationARView("Chicago");

            yield return new WaitForSeconds(0.1f);

            var location = PlayerPrefs.GetString("location");
        
            Assert.AreEqual("ARView", SceneManager.GetActiveScene().name);
            Assert.AreEqual("Chicago", location);
        }

        [UnityTest]
        public IEnumerator GivenIowaForgeButtonClickLoadARViewForIowa()
        {
            yield return SetupScene();

            _initScript.OnClick_LoadLocationARView("Iowa");

            yield return new WaitForSeconds(0.1f);
        
            var location = PlayerPrefs.GetString("location");

            Assert.AreEqual("ARView", SceneManager.GetActiveScene().name);
            Assert.AreEqual("Iowa", location);
        }
    }

    internal class MockCompass : ICompass
    {
        public bool IsEnabled => true;
        public float TrueHeading { get; set; } = 100f;
    }
}