using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerBehaviour : MonoBehaviour
{
    public GameObject ArMarkerPrefab;
    public GameObject ArCameraComponent;
    public GameObject MapMarkerPrefab;
    public List<GameObject> ArMarkers { get; private set; }
    public List<GameObject> MapMarkers { get; private set; }

    public void Start()
    {
        ArMarkers = new List<GameObject>();
        MapMarkers = new List<GameObject>();
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
        foreach (var clonedMarker in ArMarkers)
        {
            clonedMarker.transform.LookAt(ArCameraComponent.transform);
            clonedMarker.transform.Rotate(90,0,0);
        }
    }
}