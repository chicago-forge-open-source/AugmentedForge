using System.Collections;
using AugmentedForge;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class AppInitPlayTests
{
    [UnityTest]
    public IEnumerator GivenChicagoForgeButtonClickLoadChicagoARView()
    {
        SceneManager.LoadScene("InitScene");
        yield return null;

        var camera = GameObject.Find("Main Camera");
        var initScript = camera.GetComponent<InitializeThings>();
        initScript.Compass = new MockCompass();

        initScript.OnClick_LoadLocationARView("ChicagoARView");

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("ChicagoARView", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator GivenIowaForgeButtonClickLoadIowaARView()
    {
        SceneManager.LoadScene("InitScene");
        yield return null;

        var camera = GameObject.Find("Main Camera");
        var initScript = camera.GetComponent<InitializeThings>();
        initScript.Compass = new MockCompass();

        initScript.OnClick_LoadLocationARView("IowaARView");

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("IowaARView", SceneManager.GetActiveScene().name);
    }
}

internal class MockCompass : ICompass
{
    public bool IsEnabled => true;
    public float TrueHeading { get; set; } = 100f;
}