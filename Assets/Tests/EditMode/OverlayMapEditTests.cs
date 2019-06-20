using AugmentedForge;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using UnityEngine.UI;

public class OverlayMapEditTests
{
    private const int MapRotationIncrementDivisor = 4;

    private GameObject _game;
    private readonly QuaternionEqualityComparer _quaternionComparer = new QuaternionEqualityComparer(10e-6f);
    private readonly Vector3EqualityComparer _vector3Comparer = new Vector3EqualityComparer(10e-6f);
    private readonly Camera _camera = Camera.main;
    private OverlayMap _mapScript;

    [SetUp]
    public void Setup()
    {
        _game = new GameObject();
        _game.AddComponent<SpriteRenderer>();
        _mapScript = _game.AddComponent<OverlayMap>();
        _mapScript.MapCamera = _camera;
        _mapScript.LocationMarker = new GameObject();
        _mapScript.StartPoint = new GameObject();
        _mapScript.ArSessionOrigin = new GameObject();
        _mapScript.ArCamera = _game.AddComponent<Camera>();
        _mapScript.DebugText = _game.AddComponent<Text>();
    }

    [Test]
    public void Start_WillMoveLocationMarkerToStartPoint()
    {
        var startPosition = new Vector3(3, 6, 9);
        _mapScript.StartPoint.transform.position = startPosition;

        _mapScript.Start();

        var position = _mapScript.LocationMarker.transform.position;
        var expectedVector = new Vector3(startPosition.x, 0, startPosition.z);
        Assert.AreEqual(expectedVector, position);
    }

    [Test]
    public void Start_WillMoveArOriginToStartPoint()
    {
        var startPosition = new Vector3(3, 6, 9);
        _mapScript.StartPoint.transform.position = startPosition;

        _mapScript.Start();

        var position = _mapScript.ArSessionOrigin.transform.position;
        var expectedVector = new Vector3(startPosition.x, 0, startPosition.z);
        Assert.AreEqual(expectedVector, position);
    }

    [Test]
    public void Start_WillMoveMapCameraToStartPoint()
    {
        var startPosition = new Vector3(1, 2, 3);
        _mapScript.StartPoint.transform.position = startPosition;

        _mapScript.Start();

        var position = _camera.transform.position;
        Assert.AreEqual(startPosition.x, position.x);
        Assert.AreEqual(startPosition.z, position.z);
    }

    [Test]
    public void Start_WillUpdateArSessionOriginRotationBasedOnCompass()
    {
        _mapScript.Compass = new MockCompass {TrueHeading = 180f};

        _mapScript.Start();

        var expectedCameraRotation = Quaternion.Euler(0, 180f, 0);

        Assert.That(_mapScript.ArSessionOrigin.transform.rotation,
            Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer)
        );
    }

    [Test]
    public void Update_WillMoveLocationMarkerToArCameraLocation()
    {
        _mapScript.ArCamera.transform.position = new Vector3(15, 90, 34);
        _mapScript.StartPoint.transform.position = new Vector3(100, 13, 20);
        _mapScript.Update();

        Assert.AreEqual(new Vector3(15, 13, 34), _mapScript.LocationMarker.transform.position);
    }

    [Test]
    public void Update_GivenCompassWillRotateTheMapCameraIncrementally_NorthStartPosition()
    {
        _mapScript.Compass = new MockCompass {TrueHeading = 90f};
        _camera.transform.rotation = Quaternion.Euler(90, 0, 0);
        _mapScript.MapCamera = _camera;

        _mapScript.ArCamera.transform.position = new Vector3(15, 35, 34);
        _mapScript.Update();

        var expectedCameraRotation = Quaternion.Euler(
            90,
            _mapScript.Compass.TrueHeading / MapRotationIncrementDivisor,
            0);
        Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
    }

    [Test]
    public void Update_GivenCompassWillRotateTheMapCameraIncrementally_EastStartPosition()
    {
        _mapScript.Compass = new MockCompass {TrueHeading = 180f};
        var originalCameraRotationDegrees = 90;
        var originalCameraRotation = Quaternion.Euler(90, originalCameraRotationDegrees, 0);
        _camera.transform.rotation = originalCameraRotation;
        _mapScript.MapCamera = _camera;

        _mapScript.ArCamera.transform.position = new Vector3(15, 35, 34);
        _mapScript.Update();

        var differenceInRotation = _mapScript.Compass.TrueHeading - originalCameraRotationDegrees;
        var expectedCameraRotation = Quaternion.Euler(
            90,
            originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor,
            0
        );

        Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
    }

    [Test]
    public void Update_GivenCompassWillRotateTheMapCameraIncrementally_ChangesNearNorth()
    {
        _mapScript.Compass = new MockCompass {TrueHeading = 358f};
        var originalCameraRotationDegrees = 2;
        var originalCameraRotation = Quaternion.Euler(90, originalCameraRotationDegrees, 0);
        _camera.transform.rotation = originalCameraRotation;
        _mapScript.MapCamera = _camera;

        _mapScript.ArCamera.transform.position = new Vector3(15, 35, 34);
        _mapScript.Update();

        var differenceInRotation = -4;
        var expectedCameraRotation = Quaternion.Euler(
            90,
            originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor,
            0
        );

        Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
    }

    [Test]
    public void Update_GivenCompassWillRotateTheMapCameraIncrementally_ChangesNearNorth_OtherDirection()
    {
        _mapScript.Compass = new MockCompass {TrueHeading = 2f};
        var originalCameraRotationDegrees = 358;
        var originalCameraRotation = Quaternion.Euler(90, originalCameraRotationDegrees, 0);
        _camera.transform.rotation = originalCameraRotation;
        _mapScript.MapCamera = _camera;

        _mapScript.ArCamera.transform.position = new Vector3(15, 35, 34);
        _mapScript.Update();

        var differenceInRotation = 4;
        var expectedCameraRotation = Quaternion.Euler(
            90,
            originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor,
            0
        );

        Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
    }

    [Test]
    public void Update_GivenChangeInUserLocationMoveMapCameraToSameLocation()
    {
        var arCameraPos = new Vector3(5, 1, 5);
        _mapScript.ArCamera.transform.position = arCameraPos;

        var mapCameraPos = new Vector3(10, 5, 10);
        _mapScript.MapCamera.transform.position = mapCameraPos;

        _mapScript.Update();

        var arCameraPosition = _mapScript.ArCamera.transform.position;

        Assert.AreEqual(arCameraPosition.x, _mapScript.MapCamera.transform.position.x);
        Assert.AreEqual(mapCameraPos.y, _mapScript.MapCamera.transform.position.y);
        Assert.AreEqual(arCameraPosition.z, _mapScript.MapCamera.transform.position.z);
    }

    [Test]
    public void GivenButtonToggleAndMapViewInArShowingHideTheMap()
    {
        _mapScript.MapCamera.enabled = true;
        
        _mapScript.OnClick_ToggleMapView();
        
        Assert.AreEqual(false, _mapScript.MapCamera.enabled);
    }
    
    [Test]
    public void GivenButtonToggleAndMapViewInArHidingShowTheMap()
    {
        _mapScript.MapCamera.enabled = false;
        
        _mapScript.OnClick_ToggleMapView();
        
        Assert.AreEqual(true, _mapScript.MapCamera.enabled);
    }
}

internal class MockCompass : ICompass
{
    public bool IsEnabled => true;
    public float TrueHeading { get; set; } = 100f;
}