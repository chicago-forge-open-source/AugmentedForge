using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using UnityEngine.UI;

public class MarkerBehaviourEditTests
{
    private readonly QuaternionEqualityComparer _quaternionComparer = new QuaternionEqualityComparer(10e-6f);

    private GameObject _game;
    private ArMarkerBehaviour _arMarkerBehaviour;

    [SetUp]
    public void Setup()
    {
        _game = new GameObject();
        _arMarkerBehaviour = _game.AddComponent<ArMarkerBehaviour>();
        _arMarkerBehaviour.ArCameraComponent = new GameObject();
        _arMarkerBehaviour.ArMarkerPrefab = new GameObject();
        _arMarkerBehaviour.ArMarkerPrefab.AddComponent<Text>();
    }

    [Test]
    public void Update_RotateMarkersToFaceArCameraLocation_EvenThoughTheModelFacesBackwardNaturally()
    {
        Repositories.MarkerRepository.Save(
            new[] {new Marker("north", 1, 0), new Marker("west", 0, 1)}
        );
        _arMarkerBehaviour.Start();
        _arMarkerBehaviour.ArCameraComponent.transform.position = new Vector3(0, 0, 0);

        _arMarkerBehaviour.Update();

        var westMarkerGameObject = _arMarkerBehaviour.Markers.First(marker => marker.name.Equals("west"));

        Assert.That(westMarkerGameObject
                .transform.rotation,
            Is.EqualTo(Quaternion.Euler(90, 0, 180)).Using(_quaternionComparer)
        );

        var northMarkerGameObject = _arMarkerBehaviour.Markers.First(marker => marker.name.Equals("north"));

        Assert.That(northMarkerGameObject
                .transform.rotation,
            Is.EqualTo(Quaternion.Euler(90, 0, 90)).Using(_quaternionComparer)
        );
    }
}