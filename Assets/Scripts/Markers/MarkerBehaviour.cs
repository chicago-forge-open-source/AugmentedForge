using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Markers
{
    public class MarkerBehaviour : MonoBehaviour
    {
        public GameObject ArMarkerPrefab;
        public GameObject ArCameraGameObject;
        public GameObject MapMarkerPrefab;

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
            CloneMarker(marker, MapMarkerPrefab, MapMarkers);
        }

        private void MakeArMarkerGameObject(Marker marker)
        {
            var arMarker = CloneMarker(marker, ArMarkerPrefab, ArMarkers);
            AddMarkerDistanceBehaviour(arMarker);
            AddMarkerFaceCameraBehaviour(arMarker);
        }

        private void AddMarkerDistanceBehaviour(GameObject arMarker)
        {
            var behaviour = arMarker.AddComponent<MarkerDistanceBehaviour>();
            behaviour.ArCameraGameObject = ArCameraGameObject;
        }

        private void AddMarkerFaceCameraBehaviour(GameObject arMarker)
        {
            var behaviour = arMarker.AddComponent<MarkerFaceCameraBehaviour>();
            behaviour.ArCameraGameObject = ArCameraGameObject;
        }

        private GameObject CloneMarker(Marker marker, GameObject prefab, List<GameObject> markers)
        {
            var clonedArMarker = Instantiate(prefab);
            clonedArMarker.name = marker.label;
            clonedArMarker.transform.position = new Vector3(marker.x, 0, marker.z);
            clonedArMarker.GetComponentInChildren<Text>().text = marker.label;
            markers.Add(clonedArMarker);
            return clonedArMarker;
        }

        public void Update()
        {
            if (Input.touchCount <= 0) return;

            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began) return;

            var touchPosition = ArCameraGameObject.GetComponent<Camera>().ScreenPointToRay(touch.position);

            if (Physics.Raycast(touchPosition, out var hitObject))
            {
                Debug.Log("HIT " + hitObject.transform.name);
                Debug.Log(ArMarkers.First(marker => marker.name.Equals(hitObject.transform.name)));
            }
        }
    }
}