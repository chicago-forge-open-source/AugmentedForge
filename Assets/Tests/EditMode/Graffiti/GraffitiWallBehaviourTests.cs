using System.Collections.Generic;
using Graffiti;
using NUnit.Framework;
using Tests.Mocks;
using UnityEngine;

namespace Tests.EditMode.Graffiti
{
    public class GraffitiWallBehaviourTests
    {
        private GraffitiWallBehaviour _graffitiWallBehaviour;
        private GameObject _gameObject;

        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _graffitiWallBehaviour = _gameObject.AddComponent<GraffitiWallBehaviour>();
            _graffitiWallBehaviour.graffitiTextureBehaviour = _gameObject.AddComponent<GraffitiTextureBehaviour>();
            _graffitiWallBehaviour.graffitiInputBehaviour = _gameObject.AddComponent<GraffitiInputBehaviour>();
            _graffitiWallBehaviour._arCameraComponent = _gameObject.AddComponent<Camera>();

            var sketcherCameraObject = new GameObject();
            _graffitiWallBehaviour._sketcherCamera = sketcherCameraObject.AddComponent<Camera>();
            _graffitiWallBehaviour._sketcherCamera.enabled = false;
            var sketcherCanvas = _gameObject.AddComponent<Canvas>();
            sketcherCanvas.enabled = false;
            _graffitiWallBehaviour._sketcherUI = sketcherCanvas;
            _graffitiWallBehaviour.sketcherSurface = new GameObject();

            var hudCanvas = new GameObject().AddComponent<Canvas>();
            hudCanvas.enabled = true;
            _graffitiWallBehaviour._hudCanvas = hudCanvas;
        }

        [Test]
        public void StartWillDisableWallInput()
        {
            _graffitiWallBehaviour.Start();

            Assert.IsFalse(_graffitiWallBehaviour.graffitiInputBehaviour.enabled);
        }

        [Test]
        public void Update_OnTouchSketcherCameraEnabledAndHudDisabled()
        {
            var touch = new Touch
                {position = new Vector2(4, 4), deltaPosition = new Vector2(2, 2), phase = TouchPhase.Began};
            _graffitiWallBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _graffitiWallBehaviour.physicsHandler = MockPhysicsHandler<GraffitiWallBehaviour>
                .ReturnsDetected(_graffitiWallBehaviour);
            _graffitiWallBehaviour.sketcherSurface.SetActive(false);

            _graffitiWallBehaviour.Update();

            Assert.IsTrue(_graffitiWallBehaviour.sketcherSurface.activeSelf);
            Assert.IsTrue(_graffitiWallBehaviour._sketcherCamera.enabled);
            Assert.IsTrue(_graffitiWallBehaviour._sketcherUI.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.graffitiTextureBehaviour.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.graffitiInputBehaviour.enabled);
            Assert.IsFalse(_graffitiWallBehaviour._hudCanvas.enabled);
        }

        [Test]
        public void OKOnClick_OnTouchSketcherCameraDisabledAndHudEnabled()
        {
            _graffitiWallBehaviour._sketcherCamera.enabled = true;
            _graffitiWallBehaviour._sketcherUI.enabled = true;
            _graffitiWallBehaviour.sketcherSurface.SetActive(true);
            _graffitiWallBehaviour._hudCanvas.enabled = false;

            _graffitiWallBehaviour.OkOnClick();

            Assert.IsFalse(_graffitiWallBehaviour.sketcherSurface.activeSelf);
            Assert.IsFalse(_graffitiWallBehaviour._sketcherCamera.enabled);
            Assert.IsFalse(_graffitiWallBehaviour._sketcherUI.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.graffitiTextureBehaviour.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.graffitiInputBehaviour.enabled);
            Assert.IsTrue(_graffitiWallBehaviour._hudCanvas.enabled);
        }

        [Test]
        public void Update_OnNoTouchSketcherCameraDoesNotEnable()
        {
            var touch = new Touch
                {position = new Vector2(4, 4), deltaPosition = new Vector2(2, 2), phase = TouchPhase.Began};
            _graffitiWallBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _graffitiWallBehaviour.physicsHandler = new MockPhysicsHandler<GraffitiWallBehaviour>();

            _graffitiWallBehaviour.graffitiInputBehaviour.enabled = false;

            _graffitiWallBehaviour.Update();

            Assert.IsFalse(_graffitiWallBehaviour._sketcherCamera.enabled);
            Assert.IsFalse(_graffitiWallBehaviour._sketcherUI.enabled);
            Assert.IsTrue(_graffitiWallBehaviour._hudCanvas.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.graffitiInputBehaviour.enabled);
        }
    }
}