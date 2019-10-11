using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Markers;
using UnityEngine;

namespace IoTLights
{
    public class MakerSpaceLightBehaviour : MonoBehaviour
    {
        public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();
        public PhysicsHandler physicsHandler = new UnityPhysicsHandler();
        public Camera arCamera;
        private Thing _makerSpaceLights;
        public (bool on, string color) lightState;

        public void Start()
        {
            _makerSpaceLights = new Thing("MakerSpaceLights");
            InvokeRepeating(nameof(GetStateOfLight), 0.0f, 0.25f);
        }

        public async void Update()
        {
            if (inputHandler.TouchCount <= 0) return;
            var touch = inputHandler.GetTouch(0);
            var touchPosition = arCamera.ScreenPointToRay(touch.position);
            var (hitBehaviour, _) = physicsHandler.Raycast<MakerSpaceLightBehaviour>(touchPosition);
            if (Equals(this, hitBehaviour))
            {
                var state = lightState.on ? "off" : "on";
                var desiredState = $"{{ \"state\":\"{state}\", \"color\": \"{NextColor()}\"}}";
                await _makerSpaceLights.UpdateThing(desiredState);
            }
        }

        private async Task GetStateOfLight()
        {
            var state = await _makerSpaceLights.GetThing();
            lightState = (on: state.state.Equals("on"), state.color);
        }

        private string NextColor()
        {
            var colors = new List<string>
            {
                "purple", "red", "green", "orange", "dark-blue", "blue", "light-blue"
            };

            if (lightState.color == null) return colors[0];
            var nextIndex = colors.FindIndex(color => color == lightState.color) + 1;
            return nextIndex > colors.Count ? colors[0] : colors[nextIndex];
        }
    }
}