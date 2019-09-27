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
    public GraffitiTextureBehaviour graffitiTextureBehaviour;

    public void Update()
    {
        HandleTouch();
    }

    private void HandleTouch()
    {
        if (inputHandler.TouchCount > 0)
            HandleTouchAtPosition(inputHandler.GetTouch(0).position, () => { EnableSketchMode(); });
    }

    private void EnableSketchMode()
    {
        _hudCanvas.enabled = false;
        graffitiTextureBehaviour.enabled = true;
        _sketcherCamera.enabled = true;
        _sketcherUI.enabled = true;
    }
    
    private void DisableSketchMode()
    {
        _hudCanvas.enabled = true;
        graffitiTextureBehaviour.enabled = false;
        _sketcherCamera.enabled = false;
        _sketcherUI.enabled = false;
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

        SaveTheWall();
    }

    public void SaveTheWall()
    {
        Debug.Log(Application.persistentDataPath);
    }
}