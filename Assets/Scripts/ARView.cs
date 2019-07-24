using System;
using System.Collections;
using AugmentedForge;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using static Helpers;

public class ARView : MonoBehaviour
{
    public GameObject ArCameraComponent;
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

    private void LocationSync()
    {
        var syncPos = StartPoint.transform.position;
        SetObjectXzPosition(ArSessionOrigin.transform, syncPos.x, syncPos.z);
        ArSessionOrigin.transform.rotation = Quaternion.Euler(0, Compass.TrueHeading, 0);
    }

}

internal class RealCompass : ICompass
{
    public bool IsEnabled => Math.Abs(TrueHeading) > 0;
    public float TrueHeading => Input.compass.trueHeading;
}


public static class Helpers
{
    public static void SetObjectXzPosition(Transform objectTransform, float x, float z)
    {
        var position = new Vector3(x, objectTransform.position.y, z);
        objectTransform.position = position;
    }
}