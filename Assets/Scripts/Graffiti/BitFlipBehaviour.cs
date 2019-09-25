using System;
using System.Collections.Generic;
using UnityEngine;

namespace Graffiti
{
    public class BitFlipBehaviour : MonoBehaviour
    {
        public Camera sketcherCamera;
        public Material material;

        public void Update()
        {
            var texture = new Texture2D(10, 10);

            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, Color.black);
                }
            }
            texture.Apply();

            material.mainTexture = texture;
        }
    }
}