using System;
using System.Collections;
using AugmentedForge;
using UnityEngine;
using UnityEngine.UI;

public class OverlayMapInitialize : MonoBehaviour
{
    public Camera MapCamera;
    public Camera ArCamera;
    public GameObject LocationMarker;
    public Text DebugText;
    public GameObject StartPoint;
    public GameObject ArSessionOrigin;
    public ICompass Compass = new RealCompass();
    public ILocationInfo StartPosition = new RealLocationInfo();

    // mHub Center GPS: 41.895888, -87.651995
    // StartPoint GPS: 41.895720, -87.651667
    //                 41.895680  -87.651569
    public void Start()
    {
        LocationSync();
    }

    public void Update()
    {
        UpdateLocationMarker();
        UpdateMapCameraRotation();
        var logLine = "ARCamera: " + ArCamera.transform.position;
        logLine += "\nGPS: " + StartPosition.Latitude + ", " + StartPosition.Longitude;
        DebugText.text = logLine;
        Debug.Log(logLine);
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
}

internal class RealCompass : ICompass
{
    public bool IsEnabled => Math.Abs(TrueHeading) > 0;
    public float TrueHeading => Input.compass.trueHeading;
}

internal class RealLocationInfo : ILocationInfo
{
    public float Latitude => Input.location.lastData.latitude;
    public float Longitude => Input.location.lastData.longitude;
}
