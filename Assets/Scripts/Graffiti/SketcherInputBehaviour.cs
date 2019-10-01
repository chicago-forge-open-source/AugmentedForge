using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Markers;
using UnityEngine;

namespace Graffiti
{
    public class SketcherInputBehaviour : MonoBehaviour
    {
        public Camera sketcherCamera;
        public PlaneTouchHandler touchHandler = new PlaneTouchHandler();
        public TextureBehaviour sketcherTextureBehaviour;
        public TextureBehaviour graffitiTextureBehaviour;

        private int TextureSize => sketcherTextureBehaviour.textureSize;

        public void Update()
        {
            HandleTouch();
        }

        private void HandleTouch()
        {
            var percentageOfWall = touchHandler.FindTouchedPoint(transform, sketcherCamera, TextureSize);
            if (percentageOfWall.HasValue)
            {
                sketcherTextureBehaviour.LitPoints.Add(percentageOfWall.Value);
            }
        }

        public void SaveBits()
        {
            var data = string.Join("", sketcherTextureBehaviour.LitPoints.Select(point =>
                $"{(int) Math.Round(point.x)},{(int) Math.Round(point.y)}\n"
            ));

            File.WriteAllBytes(Application.persistentDataPath + "/SavedImage.csv",
                Encoding.UTF8.GetBytes(data)
            );

            graffitiTextureBehaviour.LitPoints.Clear();
            graffitiTextureBehaviour.LitPoints.AddRange(sketcherTextureBehaviour.LitPoints);
        }

        public void ClearOnClick()
        {
            sketcherTextureBehaviour.LitPoints.Clear();
        }
    }
}