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

    public void Start()
    {
        LocationSync(StartPoint);
        ArSessionOrigin.transform.rotation = Quaternion.Euler(0, Compass.TrueHeading, 0);
    }

    public void Update()
    {
        UpdateLocationMarker();
        UpdateMapCamera();
    }

    public void OnApplicationFocus(bool hasFocus)
    {
        StartCoroutine(WaitForCompassEnable());
    }

    private IEnumerator WaitForCompassEnable()
    {
        yield return new WaitUntil(() => Compass.IsEnabled);
    }

    private void UpdateMapCamera()
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
        var locationX = startPointPosition.x + arCameraPosition.x;
        var locationZ = startPointPosition.z + arCameraPosition.z;
        LocationMarker.transform.position = new Vector3(locationX, startPointPosition.y, locationZ);

        var logLine = "ARCamera: " + arCameraPosition;
        DebugText.text = logLine;
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


    private void LocationSync(GameObject syncPoint)
    {
        var syncPos = syncPoint.transform.position;
        SetObjectXzPosition(LocationMarker.transform, syncPos.x, syncPos.z);
        SetObjectXzPosition(MapCamera.transform, syncPos.x, syncPos.z);
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