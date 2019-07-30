using UnityEngine;

namespace Assets.Scripts
{
    public interface InputHandler
    {
        int TouchCount { get; }

        Touch GetTouch(int index);
    }
}