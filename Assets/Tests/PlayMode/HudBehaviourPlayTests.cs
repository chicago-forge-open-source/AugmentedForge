using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class HudBehaviourPlayTests
    {
        [UnityTest]
        public IEnumerator HudBehaviourPlayTestsWithEnumeratorPasses()
        {
            SceneManager.LoadScene("ARView");
            yield return null;

            GameObject.Find("Heads-up Display").GetComponent<HudBehaviour>().OnClickBack();

            yield return null;

            Assert.AreEqual("InitScene", SceneManager.GetActiveScene().name);
        }
    }
}