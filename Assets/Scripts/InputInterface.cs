using UnityEngine;

namespace AugmentedForge
{
    public interface IInput
    {
        int TouchCount { get; }

        Touch GetTouch(int index);
    }
}