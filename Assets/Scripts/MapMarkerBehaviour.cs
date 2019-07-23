using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AugmentedForge
{
    public class MapMarkerBehaviour : MonoBehaviour
    {
        public GameObject MapMarkerPrefab;

        public List<GameObject> Markers { get; private set; }

        public void Start()
        {
            Markers = new List<GameObject>();
            var markers = Repositories.MarkerRepository.Get();
            foreach (var marker in markers)
            {
                var clonedMapMarker = Instantiate(MapMarkerPrefab);
                clonedMapMarker.name = marker.label;
                clonedMapMarker.transform.position = new Vector3(marker.x, 0, marker.z);
                foreach (var label in clonedMapMarker.GetComponentsInChildren<Text>())
                {
                    label.text = marker.label;
                }

                Markers.Add(clonedMapMarker);
            }
        }
    }
}