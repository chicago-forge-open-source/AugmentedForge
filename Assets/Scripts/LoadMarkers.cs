using UnityEngine;
using UnityEngine.UI;

public class LoadMarkers : MonoBehaviour
{
    public GameObject Marker;

    public void Start()
    {
        var markers = Repositories.MarkerRepository.Get();
        foreach (var marker in markers)
        {
            var clonedMapMarker = Instantiate(Marker);
            clonedMapMarker.name = marker.label;
            clonedMapMarker.transform.position = new Vector3(marker.x, 0, marker.z);
            foreach (var label in clonedMapMarker.GetComponentsInChildren<Text>())
            { label.text = marker.label; }
        }
    }
}