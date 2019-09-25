using AR;
using DataLoaders;
using Markers;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class OverlayMapBehaviour : MonoBehaviour
{
    private static int thing = DoThing();

    private static int DoThing()
    {
        if (Repositories.SyncPointRepository.Get().Length == 0)
        {
            new ChicagoDataLoader().DataLoad();
        }

        return 0;
    }

    public GameObject arCameraGameObject;
    public GameObject mapCameraGameObject;
    public GameObject startPoint;
    public GameObject locationMarker;
    public PresentMarkersBehaviour presentMarkersBehaviour;
    private ARCameraBackground _cameraBackground;
    public ICompass compass = new RealCompass();

    public void Start()
    {
        var location = Repositories.LocationsRepository.GetLocationByName();

        SetStartPositionBasedOnSyncPoint();
        _cameraBackground = arCameraGameObject.GetComponent<ARCameraBackground>();
        var spritePath = $"Sprites/{location.mapFileName}";
        var mapObject = (GameObject) Resources.Load(spritePath);
        GetComponent<SpriteRenderer>().sprite = mapObject.GetComponent<SpriteRenderer>().sprite;
        gameObject.transform.rotation = location.rotation;
        LocationSync();
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
        if (_cameraBackground != null && _cameraBackground.enabled)
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
        if (_cameraBackground != null && !_cameraBackground.enabled) return;
        const float divisor = 4f;
        var compassHeading = compass.TrueHeading;
        var mapCameraVector = mapCameraGameObject.transform.rotation.eulerAngles;
        var rotationDifference = CalculateRotationDifference(compassHeading, mapCameraVector);

        var finalRotation = mapCameraVector.y + rotationDifference / divisor;
        var mapRotation = Quaternion.Euler(90, finalRotation, 0);
        mapCameraGameObject.transform.rotation = mapRotation;

        foreach (var mapMarker in presentMarkersBehaviour.MapMarkers)
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