using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArDetectImageBehaviour : MonoBehaviour
{
    public ARTrackedImageManager imageManager;
    public Text debugText;
    public GameObject imageMarker;
    
    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateInfo(trackedImage);
        }
        
        foreach (var trackedImage in eventArgs.updated)
            UpdateInfo(trackedImage);
        
        var first = eventArgs.added.FirstOrDefault() ?? eventArgs.updated.FirstOrDefault();

        if (first != null && first.trackingState != TrackingState.None)
        {
            var firstTransform = first.transform;
            var logLine = "";
            logLine += $"\nSize: {first.size}";
            logLine += $"\nExtents: {first.extents}";
            var firstTransformPosition = firstTransform.position;
            logLine += $"\nPosition: {firstTransformPosition}";
            logLine += $"\nOrientation: {firstTransform.rotation}";
            debugText.text = logLine;
            imageMarker.transform.position = firstTransformPosition;
        }
    }

    void UpdateInfo(ARTrackedImage trackedImage)
    {
        Debug.Log("DID THE THING");
        Debug.Log("name " +trackedImage.name);
        Debug.Log("size " +trackedImage.size);
        Debug.Log("extents " +trackedImage.extents);
        Debug.Log("tracking state " +trackedImage.trackingState);
        Debug.Log("trackable id " +trackedImage.trackableId);
        
        var planeGo = trackedImage.transform.gameObject;

        if (trackedImage.trackingState != TrackingState.None)
        {
            planeGo.SetActive(true);
            trackedImage.transform.localScale = new Vector3(trackedImage.size.x, 1f, trackedImage.size.y);
//            var material = planeGo.GetComponentInChildren<MeshRenderer>().material;
//            material.mainTexture = fireReferenceImage.texture;
        }
        else
        {
            planeGo.SetActive(false);
        }
    }
}
