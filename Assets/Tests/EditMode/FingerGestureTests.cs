using NUnit.Framework;
using UnityEngine;

public class FingerGestureTests
{
    private GameObject _game;
    
    private Camera _camera;
    private FingerGestures _gesturesScript;

    [SetUp]
    public void Setup()
    {
        _game = new GameObject();
        _camera = _game.AddComponent<Camera>();
        _gesturesScript = _game.AddComponent<FingerGestures>();
    }

    [Test]
    public void Update_NoTouchWillNotChangeTheZoom()
    {
        var cameraZoom = _camera.fieldOfView;

        _gesturesScript.Update();

        Assert.AreEqual(cameraZoom, _camera.fieldOfView);
    }
}