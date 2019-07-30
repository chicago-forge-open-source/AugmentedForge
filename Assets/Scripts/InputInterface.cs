using UnityEngine;

namespace Assets.Scripts
{
    public interface IInput
    {
        int TouchCount { get; }

        Touch GetTouch(int index);
    }
}