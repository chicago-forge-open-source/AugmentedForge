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

        private static readonly Vector3 ChicagoSyncPointPosition = new Vector3(26.94955f, 0, -18.17933f);
        private static readonly Vector3 IowaSyncPointPosition = new Vector3(0, 0, -18.17933f);
        
        public void Start()
        {
            DetermineSyncPointPositionBasedOnLocation(PlayerPrefs.GetString("location"));
            
            LocationSync();
        }
        
        private void DetermineSyncPointPositionBasedOnLocation(String location)
        {
            var syncPoint = Repositories.SyncPointRepository.Get(location);
            var startPointPosition = new Vector3(syncPoint.X, 0, syncPoint.Z);
            StartPoint.transform.position = startPointPosition;
        }

        public void Update()
        {
            var logLine = "ARCamera: " + ArCameraComponent.transform.position;
            DebugText.text = logLine;
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