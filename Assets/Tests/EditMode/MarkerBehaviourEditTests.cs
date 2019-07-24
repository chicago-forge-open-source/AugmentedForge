using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using UnityEngine.UI;

public class MarkerBehaviourEditTests
{
    private readonly QuaternionEqualityComparer _quaternionComparer = new QuaternionEqualityComparer(10e-6f);

    private GameObject _game;
    private MarkerBehaviour _markerBehaviour;

    [SetUp]
    public void Setup()
    {
        _game = new GameObject();
        _markerBehaviour = _game.AddComponent<MarkerBehaviour>();
        _markerBehaviour.ArCameraComponent = new GameObject();
        _markerBehaviour.ArMarkerPrefab = new GameObject();
        _markerBehaviour.ArMarkerPrefab.AddComponent<Text>();
        _markerBehaviour.MapMarkerPrefab = new GameObject();
        _markerBehaviour.MapMarkerPrefab.AddComponent<Text>();
    }

    [Test]
    public void Start_MarkersAreDuplicatedAcrossLists()
    {
        Repositories.MarkerRepository.Save(
            new[] {new Marker("north", 1, 0), new Marker("west", 0, 1)}
        );
        
        _markerBehaviour.Start();
        
        Assert.True(_markerBehaviour.ArMarkers.First(marker => marker.name.Equals("north")));
        Assert.True(_markerBehaviour.MapMarkers.First(marker => marker.name.Equals("north")));
    }
    
    [Test]
    public void Update_RotateArMarkersToFaceArCameraLocation_EvenThoughTheModelFacesBackwardNaturally()
    {
        Repositories.MarkerRepository.Save(
            new[] {new Marker("north", 1, 0), new Marker("west", 0, 1)}
        );
        _markerBehaviour.Start();
        _markerBehaviour.ArCameraComponent.transform.position = new Vector3(0, 0, 0);

        _markerBehaviour.Update();

        var westMarkerGameObject = _markerBehaviour.ArMarkers.First(marker => marker.name.Equals("west"));

        Assert.That(westMarkerGameObject
                .transform.rotation,
            Is.EqualTo(Quaternion.Euler(90, 0, 180)).Using(_quaternionComparer)
        );

        var northMarkerGameObject = _markerBehaviour.ArMarkers.First(marker => marker.name.Equals("north"));

        Assert.That(northMarkerGameObject
                .transform.rotation,
            Is.EqualTo(Quaternion.Euler(90, 0, 90)).Using(_quaternionComparer)
        );
    }

    [Test]
    public void Update_RotateMapMarkersToCounteractMapRotation()
    {
        
    }
}