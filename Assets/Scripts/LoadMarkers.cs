using UnityEngine;

public class LoadMarkers : MonoBehaviour
{
    public GameObject MapMarker;
    private const int MapLayerId = 8;
    
    public void Start()
    {
        var clonedMapMarker = MapMarker;
        clonedMapMarker.name = "Test Marker";
        Instantiate(clonedMapMarker);
    }

}
