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
            if (distanceFromCameraToMarker > DistanceToHideArMarkers)
            {
                arMarker.transform.localScale = new Vector3(0,0,0);
            }
            else
            {
                arMarker.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}