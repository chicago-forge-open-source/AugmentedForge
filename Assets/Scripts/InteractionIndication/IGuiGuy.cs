using UnityEngine;

namespace InteractionIndication
{
    public interface IGuiGuy
    {
        Color Color { set; }
        Color BackGroundColor { set; }
        void DrawBox(Rect screenRect, Texture texture);
    }
}