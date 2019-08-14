using System.Linq;
using AR;
using SyncPoints;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArDetectImageBehaviour : MonoBehaviour
{
    public ARTrackedImageManager imageManager;
    public Text debugText;
    public GameObject imageMarker;
    public ArCalibrationBehaviour calibrationBehaviour;
    public GameObject arCamera;
    
    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    
    public void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.updated)
            UpdateInfo(trackedImage);
        
        var first = eventArgs.added.FirstOrDefault() ?? eventArgs.updated.FirstOrDefault();

        if (first != null && first.trackingState != TrackingState.None)
        {
            var firstTransformPosition = LogThings(first);
            MoveSanityCheckMarker(firstTransformPosition);
        }
    }

    private void MoveSanityCheckMarker(Vector3 firstTransformPosition)
    {
        imageMarker.transform.position = firstTransformPosition;
    }

    private Vector3 LogThings(ARTrackedImage first)
    {
        var firstTransform = first.transform;
        var logLine = "";
        logLine += $"\nSize: {first.size}";
        var firstTransformPosition = firstTransform.position;
        logLine += $"\nPosition: {firstTransformPosition}";
        logLine += $"\nOri: {firstTransform.rotation.eulerAngles}";
        logLine += $"\nFromCam: {arCamera.transform.position - firstTransformPosition}";
        debugText.text = logLine;
        return firstTransformPosition;
    }

    void UpdateInfo(ARTrackedImage trackedImage)
    {
        var planeGo = trackedImage.transform.gameObject;
        var syncPoint = Repositories.SyncPointRepository.Get().FirstOrDefault(element => element.Name == trackedImage.name);
        
        var syncedX = arCamera.transform.position.x - (trackedImage.transform.position.x - syncPoint.X);
        var syncedZ = arCamera.transform.position.z - (trackedImage.transform.position.z - syncPoint.Z);
        var orientation = GetSyncOrientation(syncPoint.Orientation, trackedImage.transform.rotation.eulerAngles.y);

        calibrationBehaviour.pendingSyncPoint = new SyncPoint(trackedImage.name, syncedX, syncedZ, orientation);
        
        if (trackedImage.trackingState != TrackingState.None)
        {
            planeGo.SetActive(true);
            trackedImage.transform.localScale = new Vector3(trackedImage.size.x, 1f, trackedImage.size.y);
        }
        else
        {
            planeGo.SetActive(false);
        }
    }


    float GetSyncOrientation(float fixedOrientation, float imageRotation)
    {
        var cameraRotation = arCamera.transform.rotation.eulerAngles.y;
        return fixedOrientation + 180 + (cameraRotation - imageRotation);
    }
}
