using System;
using System.Threading.Tasks;
using Markers;
using UnityEngine;

namespace IoTLights
{
    public class IoTLightBehaviour : MonoBehaviour
    {
        public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        public GameObject arCameraGameObject;
        public GameObject lightSwitch;
        private Camera _arCameraComponent;
        private Thing _ioTLight;
        public bool onOffState;

        public void Start()
        {
            _ioTLight = new Thing("IoTLight");
            _arCameraComponent = arCameraGameObject.GetComponent<Camera>();
            InvokeRepeating(nameof(PollForIoTLightState), 0.0f, 1f);
        }

        public void Update()
        {
            if (inputHandler.TouchCount <= 0) return;
            var touch = inputHandler.GetTouch(0);
            var touchPosition = _arCameraComponent.ScreenPointToRay(touch.position);
            var (hitBehaviour, _) = physicsHandler.Raycast<IoTLightBehaviour>(touchPosition);
            if (Equals(this, hitBehaviour))
            {
                var state = onOffState ? "off" : "on";
                var desiredState = $"{{ \"state\":\"{state}\"}}";
                Task.Run(async () => { await _ioTLight.UpdateThing(desiredState); })
                    .GetAwaiter()
                    .GetResult();
            }
        }

        private void PollForIoTLightState()
        {
            var newRotation = lightSwitch.transform.rotation.eulerAngles;
            newRotation.y = GetStateOfLight() ? 180f : 0f;
            lightSwitch.transform.rotation = Quaternion.Euler(newRotation);
        }

        private bool GetStateOfLight()
        {
            var state = Task.Run(async () => await _ioTLight.GetThing()).GetAwaiter().GetResult();
            onOffState = state.state.Equals("on");
            return onOffState;
        }
    }
}