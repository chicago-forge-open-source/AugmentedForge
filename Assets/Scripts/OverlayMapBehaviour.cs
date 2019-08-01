using System;
using Markers;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class OverlayMapBehaviour : MonoBehaviour
{
    public GameObject arCameraGameObject;
    public GameObject mapCameraGameObject;
    public GameObject startPoint;
    public GameObject locationMarker;
    public InitializeMarkers initializeMarkers;
    private ARCameraBackground _cameraBackground;
    public ICompass compass = new RealCompass();

        private static readonly Vector3 ChicagoSyncPointPosition = new Vector3(26.94955f, 0, -18.17933f);
        private static readonly Vector3 IowaSyncPointPosition = new Vector3(0, 0, -18.17933f);

        public void Start()
        {
            DetermineSyncPointPositionBasedOnLocation(PlayerPrefs.GetString("location"));
            
            _cameraBackground = arCameraGameObject.GetComponent<ARCameraBackground>();
            var spritePath = $"Sprites/{PlayerPrefs.GetString("location")}Map";
            var mapObject = (GameObject) Resources.Load(spritePath);
            GetComponent<SpriteRenderer>().sprite = mapObject.GetComponent<SpriteRenderer>().sprite;
            LocationSync();
        }

        private void DetermineSyncPointPositionBasedOnLocation(String location)
        {
            var syncPoint = Repositories.SyncPointRepository.Get(location);
            var startPointPosition = new Vector3(syncPoint.X, 0, syncPoint.Z);
            StartPoint.transform.position = startPointPosition;
        }

        private void LocationSync()
        {
            var syncPos = startPoint.transform.position;
            Helpers.SetObjectXzPosition(locationMarker.transform, syncPos.x, syncPos.z);
            Helpers.SetObjectXzPosition(mapCameraGameObject.transform, syncPos.x, syncPos.z);
        }

    public void Update()
    {
        UpdateLocationMarker();
        UpdateMapCameraRotation();
    }

    private void UpdateLocationMarker()
    {
        var arCameraPosition = arCameraGameObject.transform.position;

        var startPointPosition = startPoint.transform.position;
        locationMarker.transform.position = new Vector3(arCameraPosition.x, startPointPosition.y, arCameraPosition.z);
        if (_cameraBackground.enabled)
        {
            MoveOverlayMap(arCameraPosition);
        }
    }

    private void MoveOverlayMap(Vector3 newPosition)
    {
        var mapCameraPosition = mapCameraGameObject.transform.position;
        mapCameraGameObject.transform.position = new Vector3(newPosition.x, mapCameraPosition.y, newPosition.z);
    }

    private void UpdateMapCameraRotation()
    {
        if (!_cameraBackground.enabled) return;
        const float divisor = 4f;
        var compassHeading = compass.TrueHeading;
        var mapCameraVector = mapCameraGameObject.transform.rotation.eulerAngles;
        var rotationDifference = CalculateRotationDifference(compassHeading, mapCameraVector);

        var finalRotation = mapCameraVector.y + rotationDifference / divisor;
        var mapRotation = Quaternion.Euler(90, finalRotation, 0);
        mapCameraGameObject.transform.rotation = mapRotation;

        foreach (var mapMarker in initializeMarkers.MapMarkers)
        {
            mapMarker.transform.rotation = mapRotation;
        }
    }

    private static float CalculateRotationDifference(float compassHeading, Vector3 mapCameraVector)
    {
        var rotationDifference = compassHeading - mapCameraVector.y;
        if (rotationDifference > 180) rotationDifference -= 360;
        else if (rotationDifference < -180) rotationDifference += 360;
        return rotationDifference;
    }
}