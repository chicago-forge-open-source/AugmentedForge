using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graffiti
{
    public class DropGraffitiInputBehaviour : MonoBehaviour
    {
        public TextureBehaviour graffitiTextureBehaviour;
        public TextureBehaviour sketcherTextureBehaviour;
        private List<Vector2> _originalGraffitiLitPoints;
        public Vector2 dropPoint;

        public void OnEnable()
        {
            _originalGraffitiLitPoints = graffitiTextureBehaviour.LitPoints.ToList();
//         current state graffiti texture behaviour and current state of sketcher texture behaviour   
        }

        public void Update()
        {
            var offsetPoints = sketcherTextureBehaviour.LitPoints.Select(point => point + dropPoint);
            graffitiTextureBehaviour.LitPoints.AddRange(offsetPoints);
        }

        public void CancelDrop()
        {
            graffitiTextureBehaviour.LitPoints.Clear();
            graffitiTextureBehaviour.LitPoints.AddRange(_originalGraffitiLitPoints);
        }
    }
}