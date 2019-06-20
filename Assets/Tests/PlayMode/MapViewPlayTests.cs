using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MapViewPlayTests
{
    [UnityTest]
    public IEnumerator GivenChicagoARViewButtonClickLoadChicagoARView()
    {
        SceneManager.LoadScene("ChicagoMapView");
        yield return null;

        var overlayMap = GameObject.Find("Overlay Map");
        var mapViewScript = overlayMap.GetComponent<MapView>();

        mapViewScript.OnClick_LoadARView();
        
        yield return new WaitForSeconds(0.1f);
        
        Assert.AreEqual("ChicagoARView", SceneManager.GetActiveScene().name);
    }
}
