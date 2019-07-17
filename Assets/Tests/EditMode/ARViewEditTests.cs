using AugmentedForge;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARViewEditTests
{
    private const int MapRotationIncrementDivisor = 4;

    private GameObject _game;
    private readonly QuaternionEqualityComparer _quaternionComparer = new QuaternionEqualityComparer(10e-6f);
    private GameObject _camera;
    private ARView _mapScript;

    [SetUp]
    public void Setup()
    {
        _game = new GameObject();
        _game.AddComponent<SpriteRenderer>();
        _mapScript = _game.AddComponent<ARView>();
        _mapScript.DebugText = _game.AddComponent<Text>();

        _camera = new GameObject();
        _mapScript.MapCamera = _camera;
        _mapScript.MapCamera.AddComponent<Camera>();
        _mapScript.MapCamera.AddComponent<FingerGestures>();

        _mapScript.ArCamera = new GameObject();
        _mapScript.ArCamera.AddComponent<ARCameraBackground>();

        _mapScript.LocationMarker = new GameObject();
        _mapScript.StartPoint = new GameObject();
        _mapScript.ArSessionOrigin = new GameObject();
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
    public void Start_WillLoadCorrectMapSpriteBasedOnLocationSelected()
    {
        const string location = "Chicago";
        PlayerPrefs.SetString("location", location);

        _mapScript.Start();

        var spriteName = _mapScript.GetComponent<SpriteRenderer>().sprite.name;
        Assert.AreEqual(location + "MapSprite", spriteName);
    }

    [Test]
    public void Start_WillLoadCorrectMapSpriteOfDifferentLocation()
    {
        const string location = "Iowa";
        PlayerPrefs.SetString("location", location);

        _mapScript.Start();

        var spriteName = _mapScript.GetComponent<SpriteRenderer>().sprite.name;
        Assert.AreEqual(location + "MapSprite", spriteName);
    }

    [Test]
    public void Update_WillMoveLocationMarkerToArCameraLocation()
    {
        _mapScript.Start();

        _mapScript.ArCamera.transform.position = new Vector3(15, 90, 34);
        _mapScript.StartPoint.transform.position = new Vector3(100, 13, 20);
        _mapScript.Update();

        Assert.AreEqual(new Vector3(15, 13, 34), _mapScript.LocationMarker.transform.position);
    }

    [Test]
    public void Update_RotationWillNotChangeWhenARCameraBackgroundNotEnabled()
    {
        _mapScript.Start();
        _mapScript.ArCamera.GetComponent<ARCameraBackground>().enabled = false;

        _mapScript.Compass = new MockCompass {TrueHeading = 90f};
        _camera.transform.rotation = Quaternion.Euler(90, 0, 0);
        _mapScript.MapCamera = _camera;

        _mapScript.ArCamera.transform.position = new Vector3(15, 35, 34);
        _mapScript.Update();

        var expectedCameraRotation = Quaternion.Euler(90, 0, 0);
        Assert.That(_camera.transform.rotation, Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
    }

    [Test]
    public void Update_GivenCompassWillRotateTheMapCameraIncrementally_NorthStartPosition()
    {
        _mapScript.Start();

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
        _mapScript.Start();

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
        _mapScript.Start();

        _mapScript.Compass = new MockCompass {TrueHeading = 358f};
        const int originalCameraRotationDegrees = 2;
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
        _mapScript.Start();

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
    public void Update_GivenArCameraBackgroundIsDisabled_MapCameraWillNotChange()
    {
        _mapScript.Start();
        _mapScript.ArCamera.GetComponent<ARCameraBackground>().enabled = false;


        var arCameraPos = new Vector3(5, 1, 5);
        _mapScript.ArCamera.transform.position = arCameraPos;

        var mapCameraPos = new Vector3(10, 5, 10);
        _mapScript.MapCamera.transform.position = mapCameraPos;

        _mapScript.Update();

        var arCameraPosition = _mapScript.ArCamera.transform.position;

        var position = _mapScript.MapCamera.transform.position;
        Assert.AreEqual(10, position.x);
        Assert.AreEqual(5, position.y);
        Assert.AreEqual(10, position.z);
    }

    [Test]
    public void Update_GivenChangeInUserLocationMoveMapCameraToSameLocation()
    {
        _mapScript.Start();

        var arCameraPos = new Vector3(5, 1, 5);
        _mapScript.ArCamera.transform.position = arCameraPos;

        var mapCameraPos = new Vector3(10, 5, 10);
        _mapScript.MapCamera.transform.position = mapCameraPos;

        _mapScript.Update();

        var arCameraPosition = _mapScript.ArCamera.transform.position;
        var position = _mapScript.MapCamera.transform.position;

        Assert.AreEqual(arCameraPosition.x, position.x);
        Assert.AreEqual(mapCameraPos.y, position.y);
        Assert.AreEqual(arCameraPosition.z, position.z);
    }

    [Test]
    public void GivenButtonToggleAndMapViewInArShowingHideTheMap()
    {
        var camera = _mapScript.MapCamera.GetComponent<Camera>();
        camera.enabled = true;

        _mapScript.OnClick_ToggleMapView();

        Assert.IsFalse(camera.enabled);
    }

    [Test]
    public void GivenButtonToggleAndMapViewInArHidingShowTheMap()
    {
        var camera = _mapScript.MapCamera.GetComponent<Camera>();
        camera.enabled = false;

        _mapScript.OnClick_ToggleMapView();

        Assert.IsTrue(camera.enabled);
    }

    [Test]
    public void MapCameraIsAlwaysShown()
    {
        _mapScript.Start();
        var camera = _mapScript.MapCamera.GetComponent<Camera>();
        camera.enabled = false;

        _mapScript.OnClick_LoadMapOnlyView();

        Assert.IsTrue(camera.enabled);
    }

    [Test]
    public void GivenARBackgroundIsEnabled_ARBackgroundIsHidden()
    {
        _mapScript.Start();
        var background = _mapScript.ArCamera.GetComponent<ARCameraBackground>();

        _mapScript.OnClick_LoadMapOnlyView();

        Assert.IsFalse(background.enabled);
    }

    [Test]
    public void GivenARBackgroundIsHidden_ARBackgroundIsShown()
    {
        _mapScript.Start();
        var background = _mapScript.ArCamera.GetComponent<ARCameraBackground>();
        background.enabled = false;

        _mapScript.OnClick_LoadMapOnlyView();

        Assert.IsTrue(background.enabled);
    }
    
    [Test]
    public void GivenArBackgroundTogglesToDisabled_FingerGesturesEnabled()
    {
        _mapScript.Start();
        var gesturesScript = _camera.GetComponent<FingerGestures>();
        gesturesScript.enabled = false;

        _mapScript.OnClick_LoadMapOnlyView();

        Assert.IsTrue(gesturesScript.enabled);
    }
    
    [Test]
    public void GivenArBackgroundTogglesToEnabled_FingerGesturesDisabled()
    {
        _mapScript.Start();
        _mapScript.ArCamera.GetComponent<ARCameraBackground>().enabled = false;

        _mapScript.OnClick_LoadMapOnlyView();

        Assert.IsFalse(_camera.GetComponent<FingerGestures>().enabled);
    }
    
    [Test]
    public void GivenArBackgroundTogglesToEnabled_MapCameraZoomIsSetToInitialValue()
    {
        _mapScript.Start();
        _mapScript.ArCamera.GetComponent<ARCameraBackground>().enabled = false;
        var camera = _camera.GetComponent<Camera>();
        camera.fieldOfView = 80f;

        _mapScript.OnClick_LoadMapOnlyView();

        
        Assert.AreEqual(60f, camera.fieldOfView);
    }
    
    [Test]
    public void GivenArBackgroundTogglesToEnabled_MapCameraRotationIsSetToInitialValue()
    {
        _mapScript.Start();
        var camera = _camera.GetComponent<Camera>();
        camera.transform.rotation = Quaternion.Euler(100, 10, 70);

        _mapScript.OnClick_LoadMapOnlyView();

        var expectedCameraRotation = Quaternion.Euler(90, 0, 0);
        Assert.That(_camera.transform.rotation,
            Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer)
        );
    }
}

internal class MockCompass : ICompass
{
    public bool IsEnabled => true;
    public float TrueHeading { get; set; } = 100f;
}