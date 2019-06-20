using System;
using System.Collections;
using AugmentedForge;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OverlayMap : MonoBehaviour
{
    public Camera MapCamera;
    public Camera ArCamera;
    public GameObject LocationMarker;
    public Text DebugText;
    public GameObject StartPoint;
    public GameObject ArSessionOrigin;
    public ICompass Compass = new RealCompass();


    public void Start()
    {
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
        var divisor = 4f;
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
        MapCamera.transform.position = new Vector3(locationX, mapCameraPosition.y, locationZ);
    }

    private static float CalculateRotationDifference(float compassHeading, Vector3 mapCameraVector)
    {
        var rotationDifference = compassHeading - mapCameraVector.y;

        if (rotationDifference > 180)
        {
            rotationDifference -= 360;
        }
        else if (rotationDifference < -180)
        {
            rotationDifference += 360;
        }

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

    private void SetObjectXzPosition(Transform objectTransform, float x, float z)
    {
        var position = new Vector3(x, objectTransform.position.y, z);
        objectTransform.position = position;
    }

    public void OnClick_ToggleMapView()
    {
        MapCamera.enabled = !MapCamera.enabled;
    }

    public void OnClick_LoadMapOnlyView(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

internal class RealCompass : ICompass
{
    public bool IsEnabled => Math.Abs(TrueHeading) > 0;
    public float TrueHeading => Input.compass.trueHeading;
}
