using UnityEngine;

namespace Assets.Scripts
{
    public class MarkerDistanceBehaviour : MonoBehaviour
    {
        private const int DistanceToHideArMarkers = 10;

        public GameObject ArCameraGameObject;

        public void Update()
        {
            HideMarkersBasedOnDistanceFromCamera(gameObject);
        }

        private void HideMarkersBasedOnDistanceFromCamera(GameObject arMarker)
        {
            var currentCameraPosition = ArCameraGameObject.transform.position;
            var distanceFromCameraToMarker = Vector3.Distance(currentCameraPosition, arMarker.transform.position);
            arMarker.SetActive(distanceFromCameraToMarker < DistanceToHideArMarkers);
        }
    }
}