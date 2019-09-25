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
        private List<Vector2> litPoints = new List<Vector2>();

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
                
                Debug.Log($"Touch position x {touchPosition.x} + y {touchPosition.y}");
                litPoints.Add(touchPosition);
            }

            litPoints.ForEach(point =>
            {
                var targetX = (int) point.x;
                var targetY = (int) point.y;
                texture.SetPixel(targetX, targetY, Color.white);
            });
            texture.Apply();

            material.mainTexture = texture;
        }
    }
}