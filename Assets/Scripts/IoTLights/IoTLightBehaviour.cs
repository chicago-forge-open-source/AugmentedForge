using System;
using System.Threading.Tasks;
using Markers;
using UnityEngine;

namespace IoTLights
{
    public class IoTLightBehaviour : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public InputHandler inputHandler = new UnityInputHandler();
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        public GameObject arCameraGameObject;
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
                if (!onOffState)
                {
                    Task.Run(async () => { await _ioTLight.UpdateLightState("on"); })
                        .GetAwaiter()
                        .GetResult();
                }
                else
                {
                    Task.Run(async () => { await _ioTLight.UpdateLightState("off"); })
                        .GetAwaiter()
                        .GetResult();
                }
            }
        }

        private void PollForIoTLightState()
        {
            meshRenderer.material.color = GetStateOfLight();
        }

        private Color GetStateOfLight()
        {
            if (_ioTLight == null) return Color.magenta;
            var state = Task.Run(async () => await _ioTLight.GetIoTThing()).GetAwaiter().GetResult();
            onOffState = state.state.Equals("on");
            return onOffState ? Color.yellow : Color.gray;
        }
    }
}
