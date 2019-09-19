using System;
using System.Threading.Tasks;
using Markers;
using UnityEngine;

namespace IoTLights
{
    public class IoTLightBehaviour : MonoBehaviour
    {
        public InputHandler inputHandler = new UnityInputHandler();
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        public GameObject arCameraGameObject;
        public GameObject lightSwitch;
        private Camera _arCameraComponent;
        private IoTLight _ioTLight;
        public bool onOffState;

        public void Start()
        {
            _ioTLight = new IoTLight();
            _arCameraComponent = arCameraGameObject.GetComponent<Camera>();
            InvokeRepeating(nameof(PollForIoTLightState), 0.0f, 1f);
        }

        public void Update()
        {
            if (inputHandler.TouchCount <= 0) return;
            var touch = inputHandler.GetTouch(0);
            var touchPosition = _arCameraComponent.ScreenPointToRay(touch.position);
            if (Equals(this, physicsHandler.Raycast<IoTLightBehaviour>(touchPosition)))
            {
                var state = onOffState ? "off" : "on";
                Task.Run(async () => { await _ioTLight.UpdateLightState(state); })
                    .GetAwaiter()
                    .GetResult();
            }
        }

        private void PollForIoTLightState()
        {
            print("initial: " + lightSwitch.transform.rotation.eulerAngles);
            var newRotation = lightSwitch.transform.rotation.eulerAngles;
            newRotation.y = GetStateOfLight() ? 180f : 0f;
            lightSwitch.transform.rotation = Quaternion.Euler(newRotation);
            print("new: " + lightSwitch.transform.rotation.eulerAngles);
        }

        private bool GetStateOfLight()
        {
            var state = Task.Run(async () => await _ioTLight.GetIoTThing()).GetAwaiter().GetResult();
            onOffState = state.state.Equals("on");
            return onOffState;
        }
    }
}