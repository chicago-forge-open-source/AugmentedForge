using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Graffiti
{
    public class DropGraffitiInputBehaviour : MonoBehaviour
    {
        public TextureBehaviour graffitiTextureBehaviour;
        private List<Vector2> _originalGraffitiLitPoints;
        public TextureBehaviour sketcherTextureBehaviour;

        public void OnEnable()
        {
            _originalGraffitiLitPoints = graffitiTextureBehaviour.LitPoints.ToList();
//         current state graffiti texture behaviour and current state of sketcher texture behaviour   
        }

        public void Update()
        {
            // on touch, set offset to right number with input translations
            graffitiTextureBehaviour.LitPoints.AddRange(sketcherTextureBehaviour.LitPoints);
            // take the wall points,  the offset, and the sketcher points,
            // and set them on the graffiti wall texture behaviour
        }

        public void CancelDrop()
        {
            graffitiTextureBehaviour.LitPoints = _originalGraffitiLitPoints;
        }
    }
}