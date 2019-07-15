using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class MapViewEditTests
{
    
    private GameObject _game;

    private readonly Camera _camera = Camera.main;
    private MapView _mapScript;

    [SetUp]
    public void Setup()
    {
        _game = new GameObject();
        _game.AddComponent<SpriteRenderer>();
        _mapScript = _game.AddComponent<MapView>();
    }
    
    [Test]
    public void Start_WillLoadCorrectMapSpriteBasedOnLocationSelected()
    {
        const string location = "Chicago";
        PlayerPrefs.SetString("location", location);
        
        _mapScript.Start();

        var spriteName = _mapScript.GetComponent<SpriteRenderer>().sprite.name;
        Assert.AreEqual(location + "MapSprite",spriteName);
    }
    
    [Test]
    public void Start_WillLoadCorrectMapSpriteOfDifferentLocation()
    {
        const string location = "Iowa";
        PlayerPrefs.SetString("location", location);
        
        _mapScript.Start();

        var spriteName = _mapScript.GetComponent<SpriteRenderer>().sprite.name;
        Assert.AreEqual(location + "MapSprite",spriteName);
    }
}
