using System;
using Markers;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Assets.Scripts
{
    public class OverlayMapBehaviour : MonoBehaviour
    {
        public GameObject ArCameraComponent;
        public GameObject MapCameraComponent;
        public GameObject StartPoint;
        public GameObject LocationMarker;
        public InitializeMarkers InitializeMarkers;
        private ARCameraBackground _cameraBackground;
        public ICompass Compass = new RealCompass();

        private static readonly Vector3 ChicagoSyncPointPosition = new Vector3(26.94955f, 0, -18.17933f);
        private static readonly Vector3 IowaSyncPointPosition = new Vector3(0, 0, -18.17933f);

        public void Start()
        {
            DetermineSyncPointPositionBasedOnLocation(PlayerPrefs.GetString("location"));
            
            _cameraBackground = ArCameraComponent.GetComponent<ARCameraBackground>();
            var spritePath = $"Sprites/{PlayerPrefs.GetString("location")}Map";
            var mapObject = (GameObject) Resources.Load(spritePath);
            GetComponent<SpriteRenderer>().sprite = mapObject.GetComponent<SpriteRenderer>().sprite;
            LocationSync();
        }

        private void DetermineSyncPointPositionBasedOnLocation(String location)
        {
            if (location.Equals("Chicago"))
            {
                StartPoint.transform.position = ChicagoSyncPointPosition;
            }
            else
            {
                StartPoint.transform.position = IowaSyncPointPosition;
            }
        }

        private void LocationSync()
        {
            var syncPos = StartPoint.transform.position;
            Helpers.SetObjectXzPosition(LocationMarker.transform, syncPos.x, syncPos.z);
            Helpers.SetObjectXzPosition(MapCameraComponent.transform, syncPos.x, syncPos.z);
        }

        public void Update()
        {
            UpdateLocationMarker();
            UpdateMapCameraRotation();
        }

        private void UpdateLocationMarker()
        {
            var arCameraPosition = ArCameraComponent.transform.position;

            var startPointPosition = StartPoint.transform.position;
            LocationMarker.transform.position = new Vector3(arCameraPosition.x, startPointPosition.y, arCameraPosition.z);
            if (_cameraBackground.enabled)
            {
                MoveOverlayMap(arCameraPosition);
            }
        }

        private void MoveOverlayMap(Vector3 newPosition)
        {
            var mapCameraPosition = MapCameraComponent.transform.position;
            MapCameraComponent.transform.position = new Vector3(newPosition.x, mapCameraPosition.y, newPosition.z);
        }

        private void UpdateMapCameraRotation()
        {
            if (!_cameraBackground.enabled) return;
            const float divisor = 4f;
            var compassHeading = Compass.TrueHeading;
            var mapCameraVector = MapCameraComponent.transform.rotation.eulerAngles;
            var rotationDifference = CalculateRotationDifference(compassHeading, mapCameraVector);

            var finalRotation = mapCameraVector.y + rotationDifference / divisor;
            var mapRotation = Quaternion.Euler(90, finalRotation, 0);
            MapCameraComponent.transform.rotation = mapRotation;

            foreach (var mapMarker in InitializeMarkers.MapMarkers)
            {
                mapMarker.transform.rotation = mapRotation;
            }
        }

        private static float CalculateRotationDifference(float compassHeading, Vector3 mapCameraVector)
        {
            var rotationDifference = compassHeading - mapCameraVector.y;
            if (rotationDifference > 180) rotationDifference -= 360;
            else if (rotationDifference < -180) rotationDifference += 360;
            return rotationDifference;
        }
    }
}