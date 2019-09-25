using UnityEngine;

internal class UnityTouchInputHandler : InputHandler
{
    public static InputHandler BuildInputHandler()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return new UnityMouseInputHandler();
        }
        else
        {
            return new UnityTouchInputHandler();
        }
    }

    private UnityTouchInputHandler()
    {
    }

    public int TouchCount => Input.touchCount;

    public Touch GetTouch(int index)
    {
        return Input.GetTouch(index);
    }
}

internal class UnityMouseInputHandler : InputHandler
{
    public int TouchCount => Input.GetMouseButton(0) ? 1 : 0;

    public Touch GetTouch(int index)
    {
        return new Touch {position = Input.mousePosition};
    }
}