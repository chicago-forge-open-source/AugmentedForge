using UnityEngine;

namespace Assets.Scripts.Markers
{
    public class MarkerFaceCameraBehaviour : MonoBehaviour
    {
        public GameObject ArCameraGameObject;

        public void Update()
        {
            RotateMarkerToFaceCamera(gameObject);
        }

        private void RotateMarkerToFaceCamera(GameObject arMarker)
        {
            arMarker.transform.LookAt(ArCameraGameObject.transform);

            var newRotation = arMarker.transform.rotation.eulerAngles;
            newRotation.x = 0;
            newRotation.z = 0;
            arMarker.transform.rotation = Quaternion.Euler(newRotation);
        }
    }
}