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

    private IEnumerator WaitForCompassEnable()
    {
        yield return new WaitUntil(() => Compass.IsEnabled);
        SceneManager.LoadScene("ARView");
    }

    public void OnClick_LoadLocationARView(string location)
    {
        PlayerPrefs.SetString("location", location);
        StartCoroutine(WaitForCompassEnable());
    }
}


