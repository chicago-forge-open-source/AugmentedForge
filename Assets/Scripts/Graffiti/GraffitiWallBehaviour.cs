using System;
using Graffiti;
using Markers;
using UnityEngine;

public class GraffitiWallBehaviour : MonoBehaviour
{
    public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
    public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
    public Camera _arCameraComponent;
    public Camera _sketcherCamera;
    public Canvas _hudCanvas;
    public Canvas _sketcherUI;
    public Canvas dropGraffitiUI;
    public SketcherInputBehaviour sketcherInputBehaviour;
    public GameObject sketcherSurface;
    public DropGraffitiInputBehaviour dropGraffitiInputBehaviour;

    public void Start()
    {
        SwitchToARMode();
    }

    public void Update()
    {
        HandleTouch();
    }

    private void HandleTouch()
    {
        if (inputHandler.TouchCount > 0)
            HandleTouchAtPosition(inputHandler.GetTouch(0).position, EnableSketchMode);
    }

    private void EnableSketchMode()
    {
        _hudCanvas.enabled = false;
        _sketcherCamera.enabled = true;
        _sketcherUI.enabled = true;
        sketcherInputBehaviour.enabled = true;
        sketcherSurface.SetActive(true);
        gameObject.SetActive(false);
    }

    private void SwitchToARMode()
    {
        dropGraffitiUI.enabled = false;
        dropGraffitiInputBehaviour.enabled = false;
        gameObject.SetActive(true);
        _hudCanvas.enabled = true;
        _sketcherCamera.enabled = false;
        _sketcherUI.enabled = false;
        sketcherInputBehaviour.enabled = false;
        sketcherSurface.SetActive(false);
    }

    private void HandleTouchAtPosition(Vector2 touchPosition, Action callback)
    {
        if (TouchDetected(touchPosition))
        {
            callback();
        }
    }

    private bool TouchDetected(Vector2 touchPosition)
    {
        var ray = _arCameraComponent.ScreenPointToRay(touchPosition);
        var (targetBehaviour, _) = physicsHandler.Raycast<GraffitiWallBehaviour>(ray);

        var touchDetected = Equals(this, targetBehaviour);
        return touchDetected;
    }

    public void ReturnToARMode()
    {
        SwitchToARMode();
    }

    public void SwitchToDropGraffitiMode()
    {
        _sketcherCamera.enabled = true;
        gameObject.SetActive(true);
        dropGraffitiUI.enabled = true;
        dropGraffitiInputBehaviour.enabled = true;
        
        _hudCanvas.enabled = false;
        sketcherSurface.SetActive(false);
        _sketcherUI.enabled = false;
        sketcherInputBehaviour.enabled = false;
    }
}