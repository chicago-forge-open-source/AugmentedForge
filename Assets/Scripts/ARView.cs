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
    public GameObject scrollItemPrefab;
    public GameObject scrollContent;

    public void Start()
    {
        SetStartPositionBasedOnSyncPoint();
        LocationSync();
        CreateScrollViewItems();
    }

    private void SetStartPositionBasedOnSyncPoint()
    {
        var syncPoint = Repositories.SyncPointRepository.Get()[0];
        var startPointPosition = new Vector3(syncPoint.X, 0, syncPoint.Z);
        startPoint.transform.position = startPointPosition;
    }

    private void LocationSync()
    {
        var syncPos = startPoint.transform.position;
        Helpers.SetObjectXzPosition(arSessionOrigin.transform, syncPos.x, syncPos.z);
        arSessionOrigin.transform.rotation = Quaternion.Euler(0, compass.TrueHeading, 0);
    }

    private void CreateScrollViewItems()
    {
        var markers = Repositories.MarkerRepository.Get();

        foreach (var marker in markers)
        {
            var clonedMarker = Instantiate(scrollItemPrefab, scrollContent.transform);
            clonedMarker.name = marker.Label;
            clonedMarker.GetComponentInChildren<Text>().text = marker.Label;
//            clonedMarker.GetComponent<Button>().onClick.AddListener(() => OnClick_MoveCameraToMarker(marker));
        }
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