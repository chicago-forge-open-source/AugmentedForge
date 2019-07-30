using System.Collections.Generic;
using Assets.Scripts;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Tests.EditMode
{
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
            _gesturesScript.Start();
        }

        [Test]
        public void Start_ScriptIsDisabled()
        {
            Assert.IsFalse(_gesturesScript.enabled);
        }

        [Test]
        public void Update_PanIsInitiallyDisabled()
        {
            var touch = new Touch {position = new Vector2(4, 4), deltaPosition = new Vector2(2, 2)};
            _gesturesScript.input = new MockInput(new List<Touch> {touch});

            _gesturesScript.Update();

            var position = _camera.transform.position;
            Assert.AreEqual(0, position.x);
            Assert.AreEqual(0, position.y);
            Assert.AreEqual(0, position.z);
        }

        [Test]
        public void Update_ZoomIsInitiallyDisabled()
        {
            var touch1 = new Touch {position = new Vector2(4, 4), deltaPosition = new Vector2(2, 2)};
            var touch2 = new Touch {position = new Vector2(6, 6), deltaPosition = new Vector2(4, 4)};
            _gesturesScript.input = new MockInput(new List<Touch> {touch1, touch2});

            _gesturesScript.Update();

            Assert.AreEqual(60f, _camera.fieldOfView);
        }

        [Test]
        public void Update_OneTouchWillNotChangeTheZoom()
        {
            var cameraZoom = _camera.fieldOfView;
            var touch = new Touch {position = new Vector2(0, 1), deltaPosition = new Vector2(2, 4)};
            _gesturesScript.input = new MockInput(new List<Touch> {touch});

            _gesturesScript.enabled = true;
            _gesturesScript.Update();

            Assert.AreEqual(cameraZoom, _camera.fieldOfView);
        }

        [Test]
        public void Update_TwoTouchesWillChangeTheZoom()
        {
            var touch1 = new Touch {position = new Vector2(4, 4), deltaPosition = new Vector2(2, 2)};
            var touch2 = new Touch {position = new Vector2(6, 6), deltaPosition = new Vector2(4, 4)};
            _gesturesScript.input = new MockInput(new List<Touch> {touch1, touch2});

            _gesturesScript.enabled = true;
            _gesturesScript.Update();

            Assert.AreEqual(58.5857849f, _camera.fieldOfView);
        }

        [Test]
        public void Update_OneTouchWillChangeTheCameraPosition()
        {
            var touch = new Touch {position = new Vector2(4, 4), deltaPosition = new Vector2(2, 2)};
            _gesturesScript.input = new MockInput(new List<Touch> {touch});

            _gesturesScript.enabled = true;
            _gesturesScript.Update();

            var position = _camera.transform.position;
            Assert.AreEqual(-0.200000003f, position.x);
            Assert.AreEqual(-0.200000003f, position.y);
        }
    }

    internal class MockInput : IInput
    {
        private readonly List<Touch> _touches;

        public MockInput(List<Touch> touches)
        {
            _touches = touches;
        }

        public int TouchCount => _touches.Count;

        public Touch GetTouch(int index)
        {
            return _touches[index];
        }
    }
}