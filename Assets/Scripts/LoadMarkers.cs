using UnityEngine;

public class LoadMarkers : MonoBehaviour
{
    public GameObject MapMarker;

    public void Start()
    {
        var markers = Repositories.MarkerRepository.Get();
        foreach (var marker in markers)
        {
            var clonedMapMarker = Instantiate(MapMarker);
            clonedMapMarker.name = marker.label;
            clonedMapMarker.transform.position = new Vector3(marker.x, 0, marker.z);
        }
    }
}