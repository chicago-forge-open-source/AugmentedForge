using Markers;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Assets.Scripts
{
    public class ControllerBehaviour : MonoBehaviour
    {
        
        private const int MappingLayerBitMask = 1 << 9;
        private static readonly Quaternion MapNorth = Quaternion.Euler(90, 0, 0);
        
        public GameObject ArCameraComponent;
        public GameObject MapCameraComponent;
        public GameObject ArMapOverlayToggle;
        public InitializeMarkers InitializeMarkers;
        
        private ARCameraBackground _cameraBackground;
        private Camera _mapCamera;
        public void Start()
        {
            _cameraBackground = ArCameraComponent.GetComponent<ARCameraBackground>();
            _mapCamera = MapCameraComponent.GetComponent<Camera>();
        }
        
        public void OnClick_ArMapOverlayToggle()
        {
            _mapCamera.enabled = !_mapCamera.enabled;
        }

        public void OnClick_MapOnlyToggle()
        {
            if (_cameraBackground.enabled)
            {
                ShowMapOnlyView();
            }
            else
            {
                ShowArView();
            }
        }

        private void ShowMapOnlyView()
        {
            _cameraBackground.enabled = false;
            MapCameraComponent.GetComponent<FingerGestures>().enabled = true;
            _mapCamera.enabled = true;
            var arCamera = ArCameraComponent.GetComponent<Camera>();
            arCamera.cullingMask ^= MappingLayerBitMask;
            MapCameraComponent.transform.rotation = MapNorth;

            foreach (var mapMarker in InitializeMarkers.MapMarkers)
            {
                mapMarker.transform.rotation = MapNorth;
            }
            
            ArMapOverlayToggle.SetActive(false);
        }

        private void ShowArView()
        {
            _cameraBackground.enabled = true;
            MapCameraComponent.GetComponent<FingerGestures>().enabled = false;
            _mapCamera.enabled = true;
            var arCamera = ArCameraComponent.GetComponent<Camera>();
            arCamera.cullingMask ^= MappingLayerBitMask;
            _mapCamera.fieldOfView = 60;
            ArMapOverlayToggle.SetActive(true);
        }
    }
}