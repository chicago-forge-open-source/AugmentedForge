using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Graffiti
{
    public class GraffitiTextureBehaviour : MonoBehaviour
    {
        public Material material;
        public List<Vector2> LitPoints { get; } = new List<Vector2>();
        private const int TextureSize = 50;

        public void Start()
        {
            if (File.Exists(Application.persistentDataPath + "/SavedImage.csv"))
            {
                ReadTextureFromFile();
            }

            material.mainTexture = BuildGraffitiTexture();
        }

        private void ReadTextureFromFile()
        {
            var rawBytes = File.ReadAllBytes(Application.persistentDataPath + "/SavedImage.csv");
            var lines = Encoding.UTF8.GetString(rawBytes)
                .Split('\n');

            foreach (var line in lines)
            {
                if (line == "") continue;
                var pairs = line.Split(',');
                LitPoints.Add(new Vector2(int.Parse(pairs[0]), int.Parse(pairs[1])));
            }
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