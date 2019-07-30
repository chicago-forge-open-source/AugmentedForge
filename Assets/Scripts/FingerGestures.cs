using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class FingerGestures : MonoBehaviour
    {
        private Camera _camera;
        public InputHandler InputHandler = new UnityInputHandler();

        public void Start()
        {
            enabled = false;
            _camera = GetComponent<Camera>();
        }

        public void Update()
        {
            if (!enabled) return;
            if (InputHandler.TouchCount == 1) Pan();
            if (InputHandler.TouchCount == 2) Zoom();
        }

        private void Pan()
        {
            var delta = InputHandler.GetTouch(0).deltaPosition;
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
            var touchZero = InputHandler.GetTouch(0);
            var touchOne = InputHandler.GetTouch(1);

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
}