using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerBehaviour : MonoBehaviour
{
    public GameObject MarkerPrefab;
    public GameObject ArCameraComponent;

    public List<GameObject> Markers { get; private set; }

    public void Start()
    {
        Markers = new List<GameObject>();
        var markers = Repositories.MarkerRepository.Get();
        foreach (var marker in markers)
        {
            var clonedMapMarker = Instantiate(MarkerPrefab);
            clonedMapMarker.name = marker.label;
            clonedMapMarker.transform.position = new Vector3(marker.x, 0, marker.z);
            foreach (var label in clonedMapMarker.GetComponentsInChildren<Text>())
            {
                label.text = marker.label;
            }

            Markers.Add(clonedMapMarker);
        }
    }

    public void Update()
    {
        foreach (var clonedMarker in Markers)
        {
            clonedMarker.transform.LookAt(ArCameraComponent.transform);
            clonedMarker.transform.Rotate(0, 180, 0);
        }
    }
}