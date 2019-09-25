﻿using System;
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
        var touchDetected = Equals(this, physicsHandler.Raycast<GraffitiWallBehaviour>(ray));
        return touchDetected;
    }
}