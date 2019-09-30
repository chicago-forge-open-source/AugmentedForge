using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Graffiti
{
    public class TextureBehaviour : MonoBehaviour
    {
        public Material material;
        public List<Vector2> LitPoints { get; } = new List<Vector2>();
        private const int TextureSize = 50;

        public void Start()
        {
            material.mainTexture = BuildGraffitiTexture();
        }

        public void Update()
        {
            material.mainTexture = BuildGraffitiTexture();
        }

        private Texture2D BuildGraffitiTexture()
        {
            var texture = MakeBlackTexture();
            SetDrawingsOnTexture(texture);
            texture.Apply();
            return texture;
        }

        private void SetDrawingsOnTexture(Texture2D texture)
        {
            LitPoints.ForEach(point =>
            {
                var targetX = (int) Math.Round(point.x);
                var targetY = (int) Math.Round(point.y);
                texture.SetPixel(targetX, targetY, Color.white);
            });
        }

        private static Texture2D MakeBlackTexture()
        {
            var texture = new Texture2D(TextureSize, TextureSize) {filterMode = FilterMode.Point};

            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, Color.black);
                }
            }

            return texture;
        }
    }
}