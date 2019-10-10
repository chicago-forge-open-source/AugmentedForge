using Markers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class HudBehaviour : MonoBehaviour
{
    private const int MappingLayerBitMask = 1 << 9;
    private static readonly Quaternion MapNorth = Quaternion.Euler(90, 0, 0);
        
    public GameObject arCameraGameObject;
    public GameObject mapCameraGameObject;
    public GameObject arMapOverlayToggle;
    public GameObject backButton;
    public PresentMarkersBehaviour presentMarkersBehaviour;
        
    private ARCameraBackground _cameraBackground;
    private Camera _mapCamera;
    public void Start()
    {
        _cameraBackground = arCameraGameObject.GetComponent<ARCameraBackground>();
        _mapCamera = mapCameraGameObject.GetComponent<Camera>();
    }
        
    public void OnClick_ArMapOverlayToggle()
    {
        _mapCamera.enabled = !_mapCamera.enabled;
    }

    public void OnClick_MapOnlyToggle()
    {
        var arCamera = arCameraGameObject.GetComponent<Camera>();
        arCamera.cullingMask ^= MappingLayerBitMask;

        if (_cameraBackground.enabled)
        {
            ShowMapOnlyView();
            return;
        }

        ShowArView();
    }

    private void ShowMapOnlyView()
    {
        _cameraBackground.enabled = false;
        mapCameraGameObject.GetComponent<FingerGestures>().enabled = true;
        _mapCamera.enabled = true;
        mapCameraGameObject.transform.rotation = MapNorth;

        foreach (var mapMarker in presentMarkersBehaviour.MapMarkers)
        {
            mapMarker.transform.rotation = MapNorth;
        }
            
        arMapOverlayToggle.SetActive(false);
        backButton.SetActive(false);
    }

    private void ShowArView()
    {
        _cameraBackground.enabled = true;
        mapCameraGameObject.GetComponent<FingerGestures>().enabled = false;
        _mapCamera.enabled = false;
        _mapCamera.fieldOfView = 60;
        arMapOverlayToggle.SetActive(true);
        backButton.SetActive(true);
    }

    public void OnClickBack()
    {
        SceneManager.LoadScene("InitScene");
    }
}