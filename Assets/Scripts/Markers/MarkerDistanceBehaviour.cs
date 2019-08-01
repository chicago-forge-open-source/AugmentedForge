using UnityEngine;

namespace Markers
{
    public class MarkerDistanceBehaviour : MonoBehaviour
    {
        private const int DistanceToHideArMarkers = 15;

        public GameObject arCameraGameObject;

        public void Update()
        {
            HideMarkersBasedOnDistanceFromCamera(gameObject);
        }

        private void HideMarkersBasedOnDistanceFromCamera(GameObject arMarker)
        {
            var currentCameraPosition = arCameraGameObject.transform.position;
            var distanceFromCameraToMarker = Vector3.Distance(currentCameraPosition, arMarker.transform.position);
            arMarker.SetActive(distanceFromCameraToMarker < DistanceToHideArMarkers);
        }
    }
}