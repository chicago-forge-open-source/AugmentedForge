using System.Collections;
using AugmentedForge;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

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

        _initScript.OnClick_LoadLocationARView("ARView");

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("ARView", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator GivenIowaForgeButtonClickLoadIowaARView()
    {
        yield return SetupScene();

        _initScript.OnClick_LoadLocationARView("IowaARView");

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("IowaARView", SceneManager.GetActiveScene().name);
    }
}

internal class MockCompass : ICompass
{
    public bool IsEnabled => true;
    public float TrueHeading { get; set; } = 100f;
}