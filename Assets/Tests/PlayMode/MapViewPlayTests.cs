using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MapViewPlayTests
{
    
    [UnityTest]
    public IEnumerator OnStartMapSpriteWillLoadIntoOverlayMap()
    {
        SceneManager.LoadScene("ChicagoMapView");
        yield return null;
        
        var map = GameObject.Find("Overlay Map");
        var mapScript = map.GetComponent<MapView>();
        
        mapScript.Start();
        
        
        yield return null;
        var mapSprite = map.GetComponent<SpriteRenderer>().sprite;

        Assert.AreEqual("ChicagoMapSprite", mapSprite.name);
    }
    
    [UnityTest]
    public IEnumerator GivenChicagoARViewButtonClickLoadChicagoARView()
    {
        SceneManager.LoadScene("ChicagoMapView");
        yield return null;

        var overlayMap = GameObject.Find("Overlay Map");
        var mapViewScript = overlayMap.GetComponent<MapView>();

        mapViewScript.OnClick_LoadARView("ChicagoARView");
        
        yield return new WaitForSeconds(0.1f);
        
        Assert.AreEqual("ChicagoARView", SceneManager.GetActiveScene().name);
    }
}
