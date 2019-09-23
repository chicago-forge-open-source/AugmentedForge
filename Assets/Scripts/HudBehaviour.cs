using Markers;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using Toggle = UnityEngine.UI.Toggle;

public class HudBehaviour : MonoBehaviour
{
    private const int MappingLayerBitMask = 1 << 9;
    private static readonly Quaternion MapNorth = Quaternion.Euler(90, 0, 0);

    public GameObject arCameraGameObject;
    public GameObject mapCameraGameObject;
    public GameObject arMapOverlayToggle;
    public PresentMarkersBehaviour presentMarkersBehaviour;
    public BetterToggleGroup drawer;

    private ARCameraBackground _cameraBackground;
    private Camera _mapCamera;

    public void Start()
    {
        _cameraBackground = arCameraGameObject.GetComponent<ARCameraBackground>();
        _mapCamera = mapCameraGameObject.GetComponent<Camera>();
        drawer.OnChange += TglGroup_OnChange;
    }

    private void TglGroup_OnChange(Toggle newActive)
    {
        var prefabName = newActive.transform.GetChild(0).name;
        var prefab = Resources.Load<GameObject>($"prefabs/{prefabName}");
        var arPos = arCameraGameObject.transform.position;
        var newPos = new Vector3(arPos.x, 0, arPos.z);
        Instantiate(prefab, newPos, Quaternion.Euler(0, 0, 0));
        newActive.interactable = false; // THERE CAN ONLY BE ONE
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
    }

    private void ShowArView()
    {
        _cameraBackground.enabled = true;
        mapCameraGameObject.GetComponent<FingerGestures>().enabled = false;
        _mapCamera.enabled = true;
        _mapCamera.fieldOfView = 60;
        arMapOverlayToggle.SetActive(true);
    }
}