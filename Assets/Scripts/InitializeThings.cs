using System;
using System.Collections;
using AugmentedForge;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeThings : MonoBehaviour
{
    public ICompass Compass = new RealCompass();
    
    public void Awake()
    {
        Input.compass.enabled = true;
        Input.location.Start();

    }

    private IEnumerator WaitForCompassEnable(string sceneName)
    {
        yield return new WaitUntil(() => Compass.IsEnabled);
        SceneManager.LoadScene(sceneName);
    }

    public void OnClick_LoadChicagoARView()
    {
        StartCoroutine(WaitForCompassEnable("ChicagoARView"));
    }

    public void OnClick_LoadIowaARView()
    {
        StartCoroutine(WaitForCompassEnable("IowaARView"));
    }
}


