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
        public UnityPlaneTouchDetector touchDetector = new UnityPlaneTouchDetector();
        public TextureBehaviour sketcherTextureBehaviour;
        public TextureBehaviour graffitiTextureBehaviour;

        private int TextureSize => sketcherTextureBehaviour.textureSize;

        public void Update()
        {
            HandleTouch();
        }

        private void HandleTouch()
        {
            var percentageOfWall = touchDetector.FindTouchedPoint(transform, sketcherCamera, TextureSize);
            if (percentageOfWall.HasValue)
            {
                sketcherTextureBehaviour.LitPoints.Add(percentageOfWall.Value);
            }
        }

        public void ClearOnClick()
        {
            sketcherTextureBehaviour.LitPoints.Clear();
        }
    }
}