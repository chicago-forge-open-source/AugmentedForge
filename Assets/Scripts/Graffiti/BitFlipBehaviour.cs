using System;
using System.Collections.Generic;
using Markers;
using UnityEngine;

namespace Graffiti
{
    public class BitFlipBehaviour : MonoBehaviour
    {
        public Camera sketcherCamera;
        public Material material;
        public PhysicsHandler physicsHandler;
        public InputHandler inputHandler = UnityTouchInputHandler.BuildInputHandler();

        public void Update()
        {
            var texture = new Texture2D(10, 10) {filterMode = FilterMode.Point};

            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, Color.black);
                }
            }

            if (inputHandler.TouchCount > 0)
            {
                var touchPosition = inputHandler.GetTouch(0).position;
                var targetX = (int) touchPosition.x;
                var targetY = (int) touchPosition.y;
                texture.SetPixel(targetX, targetY, Color.white);
            }


            texture.Apply();

            material.mainTexture = texture;
        }
    }
}