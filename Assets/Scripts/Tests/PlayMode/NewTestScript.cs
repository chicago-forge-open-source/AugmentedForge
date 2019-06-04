using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NewTestScript
    {
        [Test]
//        [UnityPlatform(RuntimePlatform.Android)] use for running tests specifically on android
//        [UnityPlatform(RuntimePlatform.IPhonePlayer)] use for running tests specifically on ios
//        [UnityPlatform(include = new []{RuntimePlatform.Android, RuntimePlatform.IPhonePlayer})] for both
        public void NewTestScriptSimplePasses()
        {
            Debug.Log("Sucka MCs");
        }

        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            yield return null;
        }
    }
}
