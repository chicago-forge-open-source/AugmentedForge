using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Markers
{
    public class InitializeMarkers : MonoBehaviour
    {
        public GameObject arMarkerPrefab;
        public GameObject arCameraGameObject;
        public GameObject mapMarkerPrefab;

        public List<GameObject> ArMarkers { get; } = new List<GameObject>();
        public List<GameObject> MapMarkers { get; } = new List<GameObject>();

        public void Start()
        {
            var markers = Repositories.MarkerRepository.Get();
            foreach (var marker in markers)
            {
                MakeArMarkerGameObject(marker);
                MakeMapMarkerGameObject(marker);
            }
        }

        private void MakeMapMarkerGameObject(Marker marker)
        {
            CloneMarker(marker, mapMarkerPrefab, MapMarkers);
        }

        private void MakeArMarkerGameObject(Marker marker)
        {
            var arMarker = CloneMarker(marker, arMarkerPrefab, ArMarkers);
            AddMarkerControlBehaviour(marker, arMarker);
        }

        private void AddMarkerControlBehaviour(Marker marker, GameObject arMarker)
        {
            var behaviour = arMarker.AddComponent<MarkerControlBehaviour>();
            behaviour.arCameraGameObject = arCameraGameObject;
            behaviour.marker = marker;
        }

        private static GameObject CloneMarker(Marker marker, GameObject prefab, ICollection<GameObject> markers)
        {
            var clonedArMarker = Instantiate(prefab);
            clonedArMarker.name = marker.Label;
            clonedArMarker.transform.position = new Vector3(marker.X, 0, marker.Z);
            clonedArMarker.GetComponentInChildren<Text>().text = marker.Label;
            markers.Add(clonedArMarker);
            return clonedArMarker;
        }
    }
}