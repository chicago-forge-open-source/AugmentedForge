using UnityEngine;

namespace InteractionIndication
{
    public class InteractionIndicationBehaviour : MonoBehaviour
    {
        public Camera arCamera;
        public Vector3 screenPoint;
        public Texture BoxTexture;
        public IGuiGuy guiGuy = new UnityGuiGuy();

        public void Update()
        {
            screenPoint = GetScreenPointFromAttachedGameObjectPosition();
        }

        private Vector3 GetScreenPointFromAttachedGameObjectPosition()
        {
            return arCamera.WorldToScreenPoint(transform.position);
        }

        public void OnGUI()
        {
            guiGuy.Color = new Color(1.0f, 1.0f, 1.0f, 1f);
            guiGuy.BackGroundColor = new Color(1.0f, 1.0f, 1.0f, 0f);
            var myRect = new Rect
            {
                width = 1000 / screenPoint.z,
                height = 1000 / screenPoint.z,
                center = new Vector2(screenPoint.x, screenPoint.y)
            };

            guiGuy.DrawBox(myRect, BoxTexture);
        }
    }
}