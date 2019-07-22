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
    private ARView _mapScript;
    private const string Chicago = "Chicago";

    [SetUp]
    public void Setup()
    {
        _game = new GameObject();
        _game.AddComponent<SpriteRenderer>();
        _mapScript = _game.AddComponent<ARView>();
        _mapScript.DebugText = _game.AddComponent<Text>();

        _mapScript.MapCameraComponent = new GameObject();
        _mapScript.MapCameraComponent.AddComponent<Camera>();
        _mapScript.MapCameraComponent.AddComponent<FingerGestures>();

        _mapScript.ArCameraComponent = new GameObject();
        _mapScript.ArCameraComponent.AddComponent<ARCameraBackground>();
        _mapScript.ArCameraComponent.GetComponent<Camera>().cullingMask = 567;

        _mapScript.LocationMarker = new GameObject();
        _mapScript.StartPoint = new GameObject();
        _mapScript.ArSessionOrigin = new GameObject();
        _mapScript.ArMapOverlayToggle = new GameObject();
        _mapScript.ArMapOverlayToggle.AddComponent<Button>();

        PlayerPrefs.SetString("location", Chicago);
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

        var position = _mapScript.MapCameraComponent.transform.position;
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
        _mapScript.Start();

        var spriteName = _mapScript.GetComponent<SpriteRenderer>().sprite.name;
        Assert.AreEqual(Chicago + "MapSprite", spriteName);
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

        _mapScript.ArCameraComponent.transform.position = new Vector3(15, 90, 34);
        _mapScript.StartPoint.transform.position = new Vector3(100, 13, 20);
        _mapScript.Update();

        Assert.AreEqual(new Vector3(15, 13, 34), _mapScript.LocationMarker.transform.position);
    }

    [Test]
    public void Update_RotationWillNotChangeWhenARCameraBackgroundNotEnabled()
    {
        StartInMapOnlyMode();

        _mapScript.Compass = new MockCompass {TrueHeading = 90f};
        _mapScript.MapCameraComponent.transform.rotation = Quaternion.Euler(90, 0, 0);

        _mapScript.ArCameraComponent.transform.position = new Vector3(15, 35, 34);
        _mapScript.Update();

        var expectedCameraRotation = Quaternion.Euler(90, 0, 0);
        Assert.That(_mapScript.MapCameraComponent.transform.rotation,
            Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
    }

    [Test]
    public void Update_GivenCompassWillRotateTheMapCameraIncrementally_NorthStartPosition()
    {
        _mapScript.Start();

        _mapScript.Compass = new MockCompass {TrueHeading = 90f};
        _mapScript.MapCameraComponent.transform.rotation = Quaternion.Euler(90, 0, 0);

        _mapScript.ArCameraComponent.transform.position = new Vector3(15, 35, 34);
        _mapScript.Update();

        var expectedCameraRotation = Quaternion.Euler(
            90,
            _mapScript.Compass.TrueHeading / MapRotationIncrementDivisor,
            0);
        Assert.That(_mapScript.MapCameraComponent.transform.rotation,
            Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
    }

    [Test]
    public void Update_GivenCompassWillRotateTheMapCameraIncrementally_EastStartPosition()
    {
        _mapScript.Start();

        _mapScript.Compass = new MockCompass {TrueHeading = 180f};
        var originalCameraRotationDegrees = 90;
        var originalCameraRotation = Quaternion.Euler(90, originalCameraRotationDegrees, 0);
        _mapScript.MapCameraComponent.transform.rotation = originalCameraRotation;

        _mapScript.ArCameraComponent.transform.position = new Vector3(15, 35, 34);
        _mapScript.Update();

        var differenceInRotation = _mapScript.Compass.TrueHeading - originalCameraRotationDegrees;
        var expectedCameraRotation = Quaternion.Euler(
            90,
            originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor,
            0
        );

        Assert.That(_mapScript.MapCameraComponent.transform.rotation,
            Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
    }

    [Test]
    public void Update_GivenCompassWillRotateTheMapCameraIncrementally_ChangesNearNorth()
    {
        _mapScript.Start();

        _mapScript.Compass = new MockCompass {TrueHeading = 358f};
        const int originalCameraRotationDegrees = 2;
        var originalCameraRotation = Quaternion.Euler(90, originalCameraRotationDegrees, 0);
        _mapScript.MapCameraComponent.transform.rotation = originalCameraRotation;

        _mapScript.ArCameraComponent.transform.position = new Vector3(15, 35, 34);
        _mapScript.Update();

        var differenceInRotation = -4;
        var expectedCameraRotation = Quaternion.Euler(
            90,
            originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor,
            0
        );

        Assert.That(_mapScript.MapCameraComponent.transform.rotation,
            Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
    }

    [Test]
    public void Update_GivenCompassWillRotateTheMapCameraIncrementally_ChangesNearNorth_OtherDirection()
    {
        _mapScript.Start();

        _mapScript.Compass = new MockCompass {TrueHeading = 2f};
        var originalCameraRotationDegrees = 358;
        var originalCameraRotation = Quaternion.Euler(90, originalCameraRotationDegrees, 0);
        _mapScript.MapCameraComponent.transform.rotation = originalCameraRotation;

        _mapScript.ArCameraComponent.transform.position = new Vector3(15, 35, 34);
        _mapScript.Update();

        var differenceInRotation = 4;
        var expectedCameraRotation = Quaternion.Euler(
            90,
            originalCameraRotationDegrees + differenceInRotation / MapRotationIncrementDivisor,
            0
        );

        Assert.That(_mapScript.MapCameraComponent.transform.rotation,
            Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer));
    }

    [Test]
    public void Update_GivenArCameraBackgroundIsDisabled_MapCameraWillNotChange()
    {
        StartInMapOnlyMode();

        var arCameraPos = new Vector3(5, 1, 5);
        _mapScript.ArCameraComponent.transform.position = arCameraPos;

        var mapCameraPos = new Vector3(10, 5, 10);
        _mapScript.MapCameraComponent.transform.position = mapCameraPos;

        _mapScript.Update();

        var position = _mapScript.MapCameraComponent.transform.position;
        Assert.AreEqual(10, position.x);
        Assert.AreEqual(5, position.y);
        Assert.AreEqual(10, position.z);
    }

    [Test]
    public void Update_GivenChangeInUserLocationMoveMapCameraToSameLocation()
    {
        _mapScript.Start();

        var arCameraPos = new Vector3(5, 1, 5);
        _mapScript.ArCameraComponent.transform.position = arCameraPos;

        var mapCameraPos = new Vector3(10, 5, 10);
        _mapScript.MapCameraComponent.transform.position = mapCameraPos;

        _mapScript.Update();

        var arCameraPosition = _mapScript.ArCameraComponent.transform.position;
        var position = _mapScript.MapCameraComponent.transform.position;

        Assert.AreEqual(arCameraPosition.x, position.x);
        Assert.AreEqual(mapCameraPos.y, position.y);
        Assert.AreEqual(arCameraPosition.z, position.z);
    }

    [Test]
    public void GivenButtonToggleAndMapViewInArShowingHideTheMap()
    {
        _mapScript.Start();
        var camera = _mapScript.MapCameraComponent.GetComponent<Camera>();
        camera.enabled = true;

        _mapScript.OnClick_ArMapOverlayToggle();

        Assert.IsFalse(camera.enabled);
    }

    [Test]
    public void GivenButtonToggleAndMapViewInArHidingShowTheMap()
    {
        _mapScript.Start();
        var camera = _mapScript.MapCameraComponent.GetComponent<Camera>();
        camera.enabled = false;

        _mapScript.OnClick_ArMapOverlayToggle();

        Assert.IsTrue(camera.enabled);
    }

    [Test]
    public void MapCameraIsAlwaysShown()
    {
        _mapScript.Start();
        var camera = _mapScript.MapCameraComponent.GetComponent<Camera>();
        camera.enabled = false;

        _mapScript.OnClick_MapOnlyToggle();

        Assert.IsTrue(camera.enabled);
    }

    [Test]
    public void GivenARBackgroundIsEnabled_ARBackgroundIsHidden()
    {
        _mapScript.Start();
        var background = _mapScript.ArCameraComponent.GetComponent<ARCameraBackground>();

        _mapScript.OnClick_MapOnlyToggle();

        Assert.IsFalse(background.enabled);
    }

    [Test]
    public void GivenARBackgroundIsHidden_ARBackgroundIsShown()
    {
        _mapScript.Start();
        var background = _mapScript.ArCameraComponent.GetComponent<ARCameraBackground>();
        background.enabled = false;

        _mapScript.OnClick_MapOnlyToggle();

        Assert.IsTrue(background.enabled);
    }

    [Test]
    public void GivenArBackgroundTogglesToDisabled_FingerGesturesEnabled()
    {
        _mapScript.Start();
        var gesturesScript = _mapScript.MapCameraComponent.GetComponent<FingerGestures>();
        gesturesScript.enabled = false;

        _mapScript.OnClick_MapOnlyToggle();

        Assert.IsTrue(gesturesScript.enabled);
    }

    [Test]
    public void GivenArBackgroundTogglesToEnabled_FingerGesturesDisabled()
    {
        StartInMapOnlyMode();

        _mapScript.OnClick_MapOnlyToggle();

        Assert.IsFalse(_mapScript.MapCameraComponent.GetComponent<FingerGestures>().enabled);
    }

    [Test]
    public void GivenArBackgroundTogglesToEnabled_MapCameraZoomIsSetToInitialValue()
    {
        StartInMapOnlyMode();
        var camera = _mapScript.MapCameraComponent.GetComponent<Camera>();
        camera.fieldOfView = 80f;

        _mapScript.OnClick_MapOnlyToggle();

        Assert.AreEqual(60f, camera.fieldOfView);
    }

    [Test]
    public void GivenArBackgroundTogglesToDisabled_MapCameraRotationIsSetToInitialValue()
    {
        _mapScript.Start();
        var camera = _mapScript.MapCameraComponent.GetComponent<Camera>();
        camera.transform.rotation = Quaternion.Euler(100, 10, 70);

        _mapScript.OnClick_MapOnlyToggle();

        var expectedCameraRotation = Quaternion.Euler(90, 0, 0);
        Assert.That(_mapScript.MapCameraComponent.transform.rotation,
            Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer)
        );
    }

    [Test]
    public void GivenArBackgroundTogglesToDisabled_ArCameraMaskDoesNotIncludeLayerNine()
    {
        _mapScript.Start();

        _mapScript.OnClick_MapOnlyToggle();

        var actualMask = _mapScript.ArCameraComponent.GetComponent<Camera>().cullingMask;
        Assert.AreEqual(0, actualMask & (1 << 9));
    }

    [Test]
    public void GivenArBackgroundTogglesToEnabled_ArCameraMaskIncludesLayerNine()
    {
        StartInMapOnlyMode();
        var arCamera = _mapScript.ArCameraComponent.GetComponent<Camera>();
        arCamera.cullingMask &= ~(1 << 9);

        _mapScript.OnClick_MapOnlyToggle();

        var actualMask = _mapScript.ArCameraComponent.GetComponent<Camera>().cullingMask;
        Assert.AreNotEqual(0, actualMask & (1 << 9));
    }

    private void StartInMapOnlyMode()
    {
        _mapScript.Start();
        _mapScript.ArCameraComponent.GetComponent<ARCameraBackground>().enabled = false;
    }
    
    [Test]
    public void GivenShowingMapOnly_ArMapOverlayToggleIsDisabled()
    {
        _mapScript.Start();

        _mapScript.OnClick_MapOnlyToggle();
        Assert.IsFalse(_mapScript.ArMapOverlayToggle.activeSelf);
    }
    
    [Test]
    public void GivenShowingArView_ArMapOverlayToggleIsEnabled()
    {
        _mapScript.Start();

        _mapScript.OnClick_MapOnlyToggle();
        _mapScript.OnClick_MapOnlyToggle();
        Assert.True(_mapScript.ArMapOverlayToggle.activeSelf);
    }
}

internal class MockCompass : ICompass
{
    public bool IsEnabled => true;
    public float TrueHeading { get; set; } = 100f;
}