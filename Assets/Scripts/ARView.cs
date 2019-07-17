﻿using System;
using System.Collections;
using AugmentedForge;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARView : MonoBehaviour
{
    public GameObject MapCamera;
    public GameObject ArCamera;
    public GameObject LocationMarker;
    public Text DebugText;
    public GameObject StartPoint;
    public GameObject ArSessionOrigin;
    public ICompass Compass = new RealCompass();
    private ARCameraBackground _cameraBackground;


    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = AppDelegate.GetMapSprite();
        _cameraBackground = ArCamera.GetComponent<ARCameraBackground>();
        LocationSync();
    }

    public void Update()
    {
        UpdateLocationMarker();
        UpdateMapCameraRotation();
        var logLine = "ARCamera: " + ArCamera.transform.position;
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
        var mapCameraVector = MapCamera.transform.rotation.eulerAngles;
        var rotationDifference = CalculateRotationDifference(compassHeading, mapCameraVector);

        var finalRotation = mapCameraVector.y + rotationDifference / divisor;
        MapCamera.transform.rotation = Quaternion.Euler(90, finalRotation, 0);
    }

    private void UpdateLocationMarker()
    {
        var arCameraPosition = ArCamera.transform.position;

        var startPointPosition = StartPoint.transform.position;
        var mapCameraPosition = MapCamera.transform.position;
        var locationX = arCameraPosition.x;
        var locationZ = arCameraPosition.z;
        LocationMarker.transform.position = new Vector3(locationX, startPointPosition.y, locationZ);
        if (_cameraBackground.enabled)
        {
            MapCamera.transform.position = new Vector3(locationX, mapCameraPosition.y, locationZ);
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
        SetObjectXzPosition(MapCamera.transform, syncPos.x, syncPos.z);

        SetObjectXzPosition(ArSessionOrigin.transform, syncPos.x, syncPos.z);
        ArSessionOrigin.transform.rotation = Quaternion.Euler(0, Compass.TrueHeading, 0);
    }

    private static void SetObjectXzPosition(Transform objectTransform, float x, float z)
    {
        var position = new Vector3(x, objectTransform.position.y, z);
        objectTransform.position = position;
    }

    public void OnClick_ToggleMapView()
    {
        var mapCameraComponent = MapCamera.GetComponent<Camera>();
        mapCameraComponent.enabled = !mapCameraComponent.enabled;
    }

    public void OnClick_LoadMapOnlyView()
    {
        MapCamera.GetComponent<Camera>().enabled = true;
        var cameraBackgroundEnabled = !_cameraBackground.enabled;
        _cameraBackground.enabled = cameraBackgroundEnabled;
        MapCamera.GetComponent<FingerGestures>().enabled = !cameraBackgroundEnabled;

        if (cameraBackgroundEnabled)
        {
            MapCamera.GetComponent<Camera>().fieldOfView = 60;
        }
        else
        {
            MapCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
        }
    }
}

internal class RealCompass : ICompass
{
    public bool IsEnabled => Math.Abs(TrueHeading) > 0;
    public float TrueHeading => Input.compass.trueHeading;
}