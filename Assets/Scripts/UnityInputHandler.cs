using UnityEngine;

namespace Assets.Scripts
{
    internal class UnityInputHandler : InputHandler
    {
        public int TouchCount => Input.touchCount;

        public Touch GetTouch(int index)
        {
            return Input.GetTouch(index);
        }
    }
}