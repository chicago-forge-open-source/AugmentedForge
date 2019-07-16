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
        if (input.TouchCount != 2) return;
        var touchZero = input.GetTouch(0);
        var touchOne = input.GetTouch(1);

        var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        var prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        var touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        var deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        var fieldOfView = _camera.fieldOfView += deltaMagnitudeDiff * 0.5f;
        _camera.fieldOfView = Mathf.Clamp(fieldOfView, 15f, 150f);
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