using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

public class ARViewPlayTests
{
    private GameObject _mainCamera;
    private GameObject _arCamera;
    private GameObject _syncPoint;

    private IEnumerator LoadScene()
    {
        SceneManager.LoadScene("ARView");
        yield return null;
        _mainCamera = GameObject.Find("Map Camera");
        _arCamera = GameObject.Find("AR Camera");
        _syncPoint = GameObject.Find("Sync Point 1");
    }

    [UnityTest]
    [UnityPlatform(RuntimePlatform.Android, RuntimePlatform.IPhonePlayer)]
    public IEnumerator WhenCompassEnabledCameraIsRotated()
    {
        yield return LoadScene();
        var comparer = new QuaternionEqualityComparer(10e-6f);
        var defaultQuaternion = Quaternion.Euler(0, 0, 0);
        yield return new WaitUntil(() => Math.Abs(Input.compass.trueHeading) > 0);
        Assert.That(_mainCamera.transform.rotation, Is.Not.EqualTo(defaultQuaternion).Using(comparer));
    }

    [UnityTest]
    public IEnumerator OnStartLocationMarkerIsSetToSyncPoint()
    {
        yield return LoadScene();
        var comparer = new Vector3EqualityComparer(10e-6f);
        var locationMarker = GameObject.Find("Location Marker");

        Assert.That(_syncPoint.transform.position, Is.EqualTo(locationMarker.transform.position).Using(comparer));
    }
    
    [UnityTest]
    public IEnumerator OnStartMapSpriteWillLoadIntoOverlayMap()
    {
        yield return LoadScene();
        var map = GameObject.Find("Overlay Map");
        var mapScript = map.GetComponent<ARView>();
        const string location = "Chicago";
        PlayerPrefs.SetString("location", location);
        
        mapScript.Start();
        
        
        yield return null;
        var mapSprite = map.GetComponent<SpriteRenderer>().sprite;

        Assert.AreEqual(location + "MapSprite", mapSprite.name);
    }

    [UnityTest]
    public IEnumerator OnStartCameraViewsLocationMarker()
    {
        yield return LoadScene();
        var position = _syncPoint.transform.position;

        var cameraPos = _mainCamera.transform.position;
        yield return null;
        Assert.AreEqual(position.x, cameraPos.x);
        Assert.AreEqual(position.z, cameraPos.z);
    }

    [UnityTest]
    [UnityPlatform(RuntimePlatform.Android, RuntimePlatform.IPhonePlayer)]
    public IEnumerator OnRealWorldChangeInLocationLocationMarkerChangesPosition()
    {
        yield return LoadScene();
        var locationMarker = GameObject.Find("Location Marker");
        var initialPosition = locationMarker.transform.position;

        yield return new WaitForSeconds(5);

        Assert.AreNotEqual(initialPosition, locationMarker.transform.position);
    }
}