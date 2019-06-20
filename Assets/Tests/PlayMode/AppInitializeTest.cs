using System.Collections;
using AugmentedForge;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class AppInitializeTest
{
    [UnityTest]
    public IEnumerator GivenChicagoForgeButtonClickLoadChicagoForgeMapScene()
    {
        SceneManager.LoadScene("InitScene");
        yield return null;

        var camera = GameObject.Find("Main Camera");
        var initScript = camera.GetComponent<InitializeThings>();
        initScript.Compass = new MockCompass();

        initScript.OnClick_LoadChicagoForgeMap();

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("ChicagoMapScene", SceneManager.GetActiveScene().name);
    }
}

internal class MockCompass : ICompass
{
    public bool IsEnabled => true;
    public float TrueHeading { get; set; } = 100f;
}