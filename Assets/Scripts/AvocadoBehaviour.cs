using System;
using System.Collections;
using System.Collections.Generic;
using IoTLights;
using Markers;
using UnityEngine;
using UnityEngine.Serialization;

public class AvocadoBehaviour : MonoBehaviour
{
    public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
    public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
    public Camera arCamera;
    public AudioSource audioSource;

    void Update()
    {
        if (inputHandler.TouchCount <= 0) return;
        var touch = inputHandler.GetTouch(0);
        var touchPosition = arCamera.ScreenPointToRay(touch.position);
        var (hitBehaviour, _) = physicsHandler.Raycast<AvocadoBehaviour>(touchPosition);
        if (Equals(this, hitBehaviour))
        {
            audioSource.Play();
        }
    }
}
