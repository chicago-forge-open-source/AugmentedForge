using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ARView : MonoBehaviour
{
    public GameObject arCameraGameObject;
    public Text debugText;
    public GameObject startPoint;
    public GameObject arSessionOrigin;
    public ICompass compass = new RealCompass();

    public void Start()
    {
        startPoint = GameObject.Find(PlayerPrefs.GetString("location") + " Sync Point");

        LocationSync();
    }

    public void Update()
    {
        var logLine = $"ARCamera: {arCameraGameObject.transform.position}";
        debugText.text = logLine;
    }

    public void OnApplicationFocus(bool hasFocus)
    {
        StartCoroutine(WaitForCompassEnable());
    }

    private IEnumerator WaitForCompassEnable()
    {
        yield return new WaitUntil(() => compass.IsEnabled);
    }

    private void LocationSync()
    {
        var syncPos = startPoint.transform.position;
        Helpers.SetObjectXzPosition(arSessionOrigin.transform, syncPos.x, syncPos.z);
        arSessionOrigin.transform.rotation = Quaternion.Euler(0, compass.TrueHeading, 0);
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