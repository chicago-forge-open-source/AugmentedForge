using UnityEngine;

namespace Graffiti
{
    public class DropGraffitiInputBehaviour : MonoBehaviour
    {
        public void OnEnable()
        {
//         current state graffiti texture behaviour and current state of sketcher texture behaviour   
        }

        public void Update()
        {
            // oh touch, set offset to right number with input translations
            // tkae the wall points,  the offset, and the sketcher pionts, and set them on the graffiti wall texture behaviour
        }

        public void CancelDrop()
        {
            //reset GWT to original points
        }
    }
}