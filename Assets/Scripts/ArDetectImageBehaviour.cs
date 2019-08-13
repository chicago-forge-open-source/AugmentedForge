using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArDetectImageBehaviour : MonoBehaviour
{
    public XRReferenceImage fireReferenceImage;
    public ARTrackedImageManager imageManager;

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

            // The image extents is only valid when the image is being tracked
            trackedImage.transform.localScale = new Vector3(trackedImage.size.x, 1f, trackedImage.size.y);

            // Set the texture
            var material = planeGo.GetComponentInChildren<MeshRenderer>().material;
            material.mainTexture = fireReferenceImage.texture;
        }
        else
        {
            planeGo.SetActive(false);
        }
    }
}
