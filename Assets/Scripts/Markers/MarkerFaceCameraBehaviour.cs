using UnityEngine;

namespace Markers
{
    public class MarkerFaceCameraBehaviour : MonoBehaviour
    {
        public GameObject arCameraGameObject;

        public void Update()
        {
            RotateMarkerToFaceCamera(gameObject);
        }

        private void RotateMarkerToFaceCamera(GameObject arMarker)
        {
            arMarker.transform.LookAt(arCameraGameObject.transform);

            var newRotation = arMarker.transform.rotation.eulerAngles;
            newRotation.x = 0;
            newRotation.z = 0;
            arMarker.transform.rotation = Quaternion.Euler(newRotation);
        }
    }
}