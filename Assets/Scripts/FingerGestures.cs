using System;
using AugmentedForge;
using UnityEngine;

public class FingerGestures : MonoBehaviour
{
    private Camera _camera;
    public IInput input = new RealInput();

    public void Start()
    {
        _camera = GetComponent<Camera>();
    }

    public void Update()
    {
        if (input.TouchCount == 1)
        {
            Pan();
        }

        if (input.TouchCount == 2)
        {
            Zoom();
        }
    }

    private void Pan()
    {
        var delta = input.GetTouch(0).deltaPosition;
        var camTransform = transform;
        camTransform.Translate(DeltaTimesSpeed(delta.x), DeltaTimesSpeed(delta.y), 0);

        var position = camTransform.position;
        camTransform.position = new Vector3(
            Clamp(position.x),
            position.y,
            Clamp(position.z)
        );
    }

    private static float DeltaTimesSpeed(float delta)
    {
        return -delta * 0.1f;
    }

    private void Zoom()
    {
        var touchZero = input.GetTouch(0);
        var touchOne = input.GetTouch(1);

        var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        var prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        var touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        var deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        var fieldOfView = _camera.fieldOfView += deltaMagnitudeDiff * 0.5f;
        _camera.fieldOfView = Clamp(fieldOfView, 15, 150);
    }
    
    private static float Clamp(float value, float min = -50, float max = 50)
    {
        return Mathf.Clamp(value, min, max);
    }
}

internal class RealInput : IInput
{
    public int TouchCount => Input.touchCount;

    public Touch GetTouch(int index)
    {
        return Input.GetTouch(index);
    }
}