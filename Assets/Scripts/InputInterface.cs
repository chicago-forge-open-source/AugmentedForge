using UnityEngine;

public interface InputHandler
{
    int TouchCount { get; }

    Touch GetTouch(int index);
}