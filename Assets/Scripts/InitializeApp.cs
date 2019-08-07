using System;
using System.Collections;
using System.Collections.Generic;
using DataLoaders;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeApp : MonoBehaviour
{
    public ICompass compass = new RealCompass();
    public DataLoader dataLoader;
    public Vector3 syncPointLocation;
    public void Awake()
    {
        Input.compass.enabled = true;
        Input.location.Start();
    }

    void Start()
    {
        Branch.initSession(BranchCallbackWithParams);
    }
    
    public void BranchCallbackWithParams(Dictionary<string, object> parameters, string error)
    {
        
        if (CanSetPlayerStartingPoint(parameters))
        {
            SetPlayerStartingPoint(parameters);
            return;
        }
        PlayerSelections.startingPointProvided = false;
        PlayerSelections.startingPoint = new Vector3();
    }

    private static bool CanSetPlayerStartingPoint(Dictionary<string, object> parameters)
    {
        return parameters != null && parameters.ContainsKey("z") && parameters.ContainsKey("x");
    }

    private static void SetPlayerStartingPoint(Dictionary<string, object> parameters)
    {
        float x = (float) Convert.ToDouble(parameters["x"]);
        float z = (float) Convert.ToDouble(parameters["z"]);
        PlayerSelections.startingPoint = new Vector3(x, 0, z);
        PlayerSelections.startingPointProvided = true;
    }

    private IEnumerator WaitForCompassEnable()
    {
        yield return new WaitUntil(() => compass.IsEnabled);
        SceneManager.LoadScene("ARView");
    }

    public void OnClick_LoadLocationARView(string location)
    {
        if (location.Equals("Iowa"))
        {
            dataLoader = new IowaDataLoader();
        }
        else
        {
            dataLoader = new ChicagoDataLoader();
        }

        dataLoader.DataLoad();
        PlayerPrefs.SetString("location", location);
        StartCoroutine(WaitForCompassEnable());
    }
}