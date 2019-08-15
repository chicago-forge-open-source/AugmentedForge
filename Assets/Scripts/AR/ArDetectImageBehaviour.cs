using System;
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
    public GameObject detectedImageMarker;
    public ArCalibrationBehaviour calibrationBehaviour;
    public GameObject arCamera;
    public Func<ARTrackedImage, string> getReferenceName = image => image.referenceImage.name;
    public Func<ARTrackedImage, TrackingState> getTrackingState = image => image.trackingState;

    private void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    public void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        var first = eventArgs.added.FirstOrDefault() ?? eventArgs.updated.FirstOrDefault();

        if (first != null && getTrackingState(first) != TrackingState.None)
        {
            UpdateInfo(first);
        }
    }

    private void MoveDetectedImageMarker(Vector3 firstTransformPosition)
    {
        detectedImageMarker.transform.position = firstTransformPosition;
    }

    private void LogThings(ARTrackedImage first)
    {
        var firstTransform = first.transform;
        var logLine = "";
        var firstTransformPosition = firstTransform.position;
        logLine += $"\nPosition: {firstTransformPosition}";
        logLine += $"\nOri: {firstTransform.rotation.eulerAngles.y}, CamOri: {arCamera.transform.rotation.eulerAngles.y}";
        logLine += $"\nFromCam: {arCamera.transform.position - firstTransformPosition}";
        debugText.text = logLine;
    }

    void UpdateInfo(ARTrackedImage trackedImage)
    {
        var trackedImageTransform = trackedImage.transform;
        var referenceImageName = getReferenceName(trackedImage);
        var syncPoint = Repositories.SyncPointRepository.Get()
            .FirstOrDefault(element => element.Name == referenceImageName);
        LogThings(trackedImage);
        MoveDetectedImageMarker(trackedImageTransform.position);

        if (syncPoint != null)
        {
            SetPendingSyncPointToSyncPoint(trackedImageTransform, syncPoint, referenceImageName);
        }

        var planeGo = trackedImageTransform.gameObject;
        if (TrackedImageHasTrackingState(trackedImage))
        {
            planeGo.SetActive(true);
            trackedImageTransform.localScale = new Vector3(trackedImage.size.x, 1f, trackedImage.size.y);
        }
        else
        {
            planeGo.SetActive(false);
        }
    }

    private static bool TrackedImageHasTrackingState(ARTrackedImage trackedImage)
    {
        return trackedImage.trackingState != TrackingState.None;
    }

    private void SetPendingSyncPointToSyncPoint(Transform trackedImageTransform, SyncPoint syncPoint,
        string referenceImageName)
    {
        var cameraPosition = arCamera.transform.position;
        var trackedImagePosition = trackedImageTransform.position;
        var syncedX = GetNewPosition(cameraPosition.x, trackedImagePosition.x, syncPoint.X);
        var syncedZ = GetNewPosition(cameraPosition.z, trackedImagePosition.z, syncPoint.Z);

        var orientation = GetSyncOrientation(
            syncPoint.Orientation,
            trackedImageTransform.rotation.eulerAngles.y
        );

        calibrationBehaviour.pendingSyncPoint = new SyncPoint(referenceImageName, syncedX, syncedZ, orientation);
    }

    private static float GetNewPosition(float camera, float image, float syncPoint)
    {
        return camera - (image - syncPoint);
    }


    private float GetSyncOrientation(float fixedOrientation, float imageRotation)
    {
        var cameraRotation = arCamera.transform.rotation.eulerAngles.y;
        return fixedOrientation + 180 + (cameraRotation - imageRotation);
    }
}