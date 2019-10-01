using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Graffiti
{
    public class DropGraffitiInputBehaviour : MonoBehaviour
    {
        public TextureBehaviour graffitiTextureBehaviour;
        public TextureBehaviour sketcherTextureBehaviour;
        public Camera sketcherCamera;
        public PlaneTouchDetector planeTouchDetector = new UnityPlaneTouchDetector();
        public Vector2 dropPoint { get; set; }
        private List<Vector2> _originalGraffitiLitPoints = new List<Vector2>();

        public void OnEnable()
        {
            var halfway = graffitiTextureBehaviour.textureSize / 2;
            dropPoint = new Vector2(halfway, halfway);
            _originalGraffitiLitPoints = graffitiTextureBehaviour.LitPoints.ToList();
        }

        public void SaveBits()
        {
            var data = string.Join("", graffitiTextureBehaviour.LitPoints.Select(point =>
                $"{(int) Math.Round(point.x)},{(int) Math.Round(point.y)}\n"
            ));

            File.WriteAllBytes(Application.persistentDataPath + "/SavedImage.csv",
                Encoding.UTF8.GetBytes(data)
            );
        }

        public void Update()
        {
            var offsetPoints = sketcherTextureBehaviour.LitPoints.Select(point => point + dropPoint);

            graffitiTextureBehaviour.LitPoints.Clear();
            graffitiTextureBehaviour.LitPoints.AddRange(_originalGraffitiLitPoints);
            graffitiTextureBehaviour.LitPoints.AddRange(offsetPoints);

            UpdateDropPoint();
        }

        private void UpdateDropPoint()
        {
            var touchedPoint = planeTouchDetector.FindTouchedPoint(transform, sketcherCamera,
                graffitiTextureBehaviour.textureSize);
            if (touchedPoint.HasValue)
                dropPoint = touchedPoint.Value;
        }

        public void CancelDrop()
        {
            graffitiTextureBehaviour.LitPoints.Clear();
            graffitiTextureBehaviour.LitPoints.AddRange(_originalGraffitiLitPoints);
        }
    }
}