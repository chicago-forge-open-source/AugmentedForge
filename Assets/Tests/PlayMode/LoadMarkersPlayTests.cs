using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

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
    public IEnumerator Start_WillLoadMarkerOntoView()
    {
        var testMarker = new Marker("Test Marker", 0, 0);

        Repositories.MarkerRepository.Save(new[] {testMarker});

        yield return LoadScene();

        AssertGameObjectCreatedCorrectly(testMarker);
    }

    [UnityTest]
    public IEnumerator Start_WillLoadMarkersFromRepoOntoView()
    {
        var testMarker1 = new Marker("Marker 1", 1, 2);
        var testMarker2 = new Marker("Marker 2", 10, 20);
        var markers = new[]
        {
            testMarker1,
            testMarker2,
        };

        Repositories.MarkerRepository.Save(markers);

        yield return LoadScene();

        AssertGameObjectCreatedCorrectly(testMarker1);
        AssertGameObjectCreatedCorrectly(testMarker2);
    }

    private static void AssertGameObjectCreatedCorrectly(Marker testMarker)
    {
        var marker = GameObject.Find(testMarker.label);
        var text = marker.GetComponentInChildren<Text>().text;
        Assert.AreEqual(testMarker.label, text);
        Assert.AreEqual(testMarker.x, marker.transform.position.x);
        Assert.AreEqual(testMarker.z, marker.transform.position.z);
    }
}