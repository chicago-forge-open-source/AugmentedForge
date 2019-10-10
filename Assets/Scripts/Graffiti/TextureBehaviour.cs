using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Graffiti
{
    public class TextureBehaviour : MonoBehaviour
    {
        public Material material;
        public List<Vector2> LitPoints { get; } = new List<Vector2>();
        public int textureSize = 50;

        public void Start()
        {
            material.mainTexture = BuildGraffitiTexture();
        }

        public void Update()
        {
            material.mainTexture = BuildGraffitiTexture();
            Resources.UnloadUnusedAssets();
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

        private Texture2D MakeBlackTexture()
        {
            var texture = new Texture2D(textureSize, textureSize) {filterMode = FilterMode.Point};

            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, Color.black);
                }
            }

            return texture;
        }

        public void SaveAsPng()
        {
            var mainTextureAs2D = ((Texture2D) material.mainTexture);
            File.WriteAllBytes(Application.persistentDataPath + "/texture_000.png", mainTextureAs2D.EncodeToPNG());
        }
    }
}