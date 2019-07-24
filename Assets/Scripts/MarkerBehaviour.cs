using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerBehaviour : MonoBehaviour
{
    public GameObject ArMarkerPrefab;
    public GameObject ArCameraComponent;
    public GameObject MapMarkerPrefab;
    public List<GameObject> ArMarkers { get; } = new List<GameObject>();
    public List<GameObject> MapMarkers { get; } = new List<GameObject>();

    private static readonly Quaternion MapNorth = Quaternion.Euler(180, 0, 0);

    public void Start()
    {
        var markers = Repositories.MarkerRepository.Get();
        foreach (var marker in markers)
        {
            CloneMarker(marker, ArMarkerPrefab, ArMarkers);
            CloneMarker(marker, MapMarkerPrefab, MapMarkers);
        }
    }

    private void CloneMarker(Marker marker, GameObject prefab, List<GameObject> markers)
    {
        var clonedArMarker = Instantiate(prefab);
        clonedArMarker.name = marker.label;
        clonedArMarker.transform.position = new Vector3(marker.x, 0, marker.z);
        clonedArMarker.GetComponentInChildren<Text>().text = marker.label;
        markers.Add(clonedArMarker);
    }

    public void Update()
    {
        foreach (var arMarker in ArMarkers)
        {
            arMarker.transform.LookAt(ArCameraComponent.transform);

            var newRotation = arMarker.transform.rotation.eulerAngles;
            newRotation.x = 0;
            newRotation.z = 0;
            arMarker.transform.rotation = Quaternion.Euler(newRotation);
        }
    }
}