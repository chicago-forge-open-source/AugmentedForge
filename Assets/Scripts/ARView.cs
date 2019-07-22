﻿using System;
using System.Collections;
using AugmentedForge;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARView : MonoBehaviour
{
    private const int MappingLayerBitMask = (1 << 9);
    public GameObject MapCameraComponent;
    public GameObject ArCameraComponent;
    public GameObject LocationMarker;
    public Text DebugText;
    public GameObject StartPoint;
    public GameObject ArSessionOrigin;
    public ICompass Compass = new RealCompass();
    private ARCameraBackground _cameraBackground;
    private Camera _mapCamera;
    private static readonly Quaternion MapNorth = Quaternion.Euler(90, 0, 0);

    public void Start()
    {
        _cameraBackground = ArCameraComponent.GetComponent<ARCameraBackground>();
        _mapCamera = MapCameraComponent.GetComponent<Camera>();

        var spritePath = $"Sprites/{PlayerPrefs.GetString("location")}Map";
        var mapObject = (GameObject) Resources.Load(spritePath);
        GetComponent<SpriteRenderer>().sprite = mapObject.GetComponent<SpriteRenderer>().sprite;

        LocationSync();
    }

    public void Update()
    {
        UpdateLocationMarker();
        UpdateMapCameraRotation();
        var logLine = "ARCamera: " + ArCameraComponent.transform.position;
        DebugText.text = logLine;
    }

    public void OnApplicationFocus(bool hasFocus)
    {
        StartCoroutine(WaitForCompassEnable());
    }

    private IEnumerator WaitForCompassEnable()
    {
        yield return new WaitUntil(() => Compass.IsEnabled);
    }

    private void UpdateMapCameraRotation()
    {
        if (!_cameraBackground.enabled) return;
        const float divisor = 4f;
        var compassHeading = Compass.TrueHeading;
        var mapCameraVector = MapCameraComponent.transform.rotation.eulerAngles;
        var rotationDifference = CalculateRotationDifference(compassHeading, mapCameraVector);

        var finalRotation = mapCameraVector.y + rotationDifference / divisor;
        MapCameraComponent.transform.rotation = Quaternion.Euler(90, finalRotation, 0);
    }

    private void UpdateLocationMarker()
    {
        var arCameraPosition = ArCameraComponent.transform.position;

        var startPointPosition = StartPoint.transform.position;
        var mapCameraPosition = MapCameraComponent.transform.position;
        var locationX = arCameraPosition.x;
        var locationZ = arCameraPosition.z;
        LocationMarker.transform.position = new Vector3(locationX, startPointPosition.y, locationZ);
        if (_cameraBackground.enabled)
        {
            MapCameraComponent.transform.position = new Vector3(locationX, mapCameraPosition.y, locationZ);
        }
    }

    private static float CalculateRotationDifference(float compassHeading, Vector3 mapCameraVector)
    {
        var rotationDifference = compassHeading - mapCameraVector.y;
        if (rotationDifference > 180) rotationDifference -= 360;
        else if (rotationDifference < -180) rotationDifference += 360;
        return rotationDifference;
    }


    private void LocationSync()
    {
        var syncPos = StartPoint.transform.position;
        SetObjectXzPosition(LocationMarker.transform, syncPos.x, syncPos.z);
        SetObjectXzPosition(MapCameraComponent.transform, syncPos.x, syncPos.z);

        SetObjectXzPosition(ArSessionOrigin.transform, syncPos.x, syncPos.z);
        ArSessionOrigin.transform.rotation = Quaternion.Euler(0, Compass.TrueHeading, 0);
    }

    private static void SetObjectXzPosition(Transform objectTransform, float x, float z)
    {
        var position = new Vector3(x, objectTransform.position.y, z);
        objectTransform.position = position;
    }

    public void OnClick_ArMapOverlayToggle()
    {
        SetMapCameraEnabled(!_mapCamera.enabled);
    }

    public void OnClick_MapOnlyToggle()
    {
        if (_cameraBackground.enabled)
        {
            ShowMapOnlyView();
        }
        else
        {
            ShowArView();
        }
    }

    private void ShowMapOnlyView()
    {
        _cameraBackground.enabled = false;
        MapCameraComponent.GetComponent<FingerGestures>().enabled = true;
        SetMapCameraEnabled(true);
        var arCamera = ArCameraComponent.GetComponent<Camera>();
        arCamera.cullingMask ^= MappingLayerBitMask;
        MapCameraComponent.transform.rotation = MapNorth;
    }

    private String bitString(int value)
    {
        return Convert.ToString(value, 2).PadLeft(32, '0');
    }

    private void ShowArView()
    {
        _cameraBackground.enabled = true;
        MapCameraComponent.GetComponent<FingerGestures>().enabled = false;
        SetMapCameraEnabled(true);
        var arCamera = ArCameraComponent.GetComponent<Camera>();
        arCamera.cullingMask ^= MappingLayerBitMask;
        _mapCamera.fieldOfView = 60;
    }

    private void SetMapCameraEnabled(bool enableMap)
    {
        _mapCamera.enabled = enableMap;
    }
}

internal class RealCompass : ICompass
{
    public bool IsEnabled => Math.Abs(TrueHeading) > 0;
    public float TrueHeading => Input.compass.trueHeading;
}