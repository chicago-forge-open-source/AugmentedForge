using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class LoadMarkersTests
{
    private GameObject _overlayMap;
    private LoadMarkers _loadScript;
    
    private IEnumerator LoadScene()
    {
        SceneManager.LoadScene("ARView");
        yield return null;

        _overlayMap = GameObject.Find("Overlay Map");
        _loadScript = _overlayMap.GetComponent<LoadMarkers>();
    }
    
    [UnityTest]
    public IEnumerator Start_WillLoadMapMarkerOntoAROverlayMap()
    {
        yield return LoadScene();
        _loadScript.MapMarker = new GameObject();
        
        var testMarker = new Marker("Test Marker", 0, 0);

        var markerRepo = new InMemoryMarkerRepository();
        markerRepo.Save(new []{testMarker});

        _loadScript.Start();
        yield return null;

        var loadedMapMarker = GameObject.Find(testMarker.label);
        Assert.AreEqual(testMarker.x, loadedMapMarker.transform.position.x);
        Assert.AreEqual(testMarker.z, loadedMapMarker.transform.position.z);
    }
}