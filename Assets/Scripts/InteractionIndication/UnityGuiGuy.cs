using UnityEngine;

namespace InteractionIndication
{
    public class UnityGuiGuy : IGuiGuy
    {
        public Color Color
        {
            set => GUI.color = value;
        }

        public Color BackGroundColor
        {
            set => GUI.backgroundColor = value;
        }

        public void DrawBox(Rect screenRect, Texture texture)
        {
            var guiRect = new Rect(screenRect)
            {
                center = new Vector2(screenRect.center.x, Screen.height - screenRect.center.y)
            };
            GUI.Box(guiRect, texture);
        }
    }
}