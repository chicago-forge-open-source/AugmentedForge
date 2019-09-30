using System;
using System.Collections;
using System.Collections.Generic;
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
    public TextureBehaviour textureBehaviour;
    public SketcherInputBehaviour sketcherInputBehaviour;
    public GameObject sketcherSurface;

    public void Start()
    {
        sketcherInputBehaviour.enabled = false;
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
        textureBehaviour.enabled = true;
        _sketcherCamera.enabled = true;
        _sketcherUI.enabled = true;
        sketcherInputBehaviour.enabled = true;
        sketcherSurface.SetActive(true);
    }

    private void DisableSketchMode()
    {
        _hudCanvas.enabled = true;
        textureBehaviour.enabled = false;
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

    public void OkOnClick()
    {
        DisableSketchMode();
    }
}