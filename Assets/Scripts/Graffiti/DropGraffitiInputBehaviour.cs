using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            _originalGraffitiLitPoints = graffitiTextureBehaviour.LitPoints.ToList();
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