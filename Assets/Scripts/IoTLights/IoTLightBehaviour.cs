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
            InvokeRepeating(nameof(PollForIoTLightState), 0.0f, 0.25f);
        }

        public async void Update()
        {
            if (inputHandler.TouchCount <= 0) return;
            var touch = inputHandler.GetTouch(0);
            var touchPosition = _arCameraComponent.ScreenPointToRay(touch.position);
            var (hitBehaviour, _) = physicsHandler.Raycast<IoTLightBehaviour>(touchPosition);
            if (Equals(this, hitBehaviour))
            {
                var state = onOffState ? "off" : "on";
                var desiredState = $"{{ \"state\":\"{state}\"}}";
                await _ioTLight.UpdateThing(desiredState);
            }
        }

        private async Task PollForIoTLightState()
        {
            var newRotation = lightSwitch.transform.rotation.eulerAngles;
            newRotation.y = await GetStateOfLight() ? 180f : 0f;
            lightSwitch.transform.rotation = Quaternion.Euler(newRotation);
        }

        private async Task<bool> GetStateOfLight()
        {
            var state =  await _ioTLight.GetThing();
            onOffState = state.state.Equals("on");
            return onOffState;
        }
    }
}