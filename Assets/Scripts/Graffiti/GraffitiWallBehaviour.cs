using System;
using System.Collections;
using System.Collections.Generic;
using Markers;
using UnityEngine;

public class GraffitiWallBehaviour : MonoBehaviour
{
    public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
    public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
    public Camera _arCameraComponent;
    public Camera _sketcherCamera;
    public Canvas _hudCanvas;

    void Start()
    {
    }

    public void Update()
    {
        HandleTouch();
    }

    private void HandleTouch()
    {
        if (inputHandler.TouchCount > 0)
            HandleTouchAtPosition(inputHandler.GetTouch(0).position, () =>
            {
                _hudCanvas.enabled = false;
                _sketcherCamera.enabled = true;
            });
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
}