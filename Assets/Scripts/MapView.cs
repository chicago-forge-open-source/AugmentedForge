using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapView : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        var mapObject = (GameObject) Resources.Load("Sprites/ChicagoMap");
        GetComponent<SpriteRenderer>().sprite = mapObject.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick_LoadARView(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
