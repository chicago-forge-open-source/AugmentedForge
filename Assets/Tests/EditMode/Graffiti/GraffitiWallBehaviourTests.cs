using System.Collections.Generic;
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
            _graffitiWallBehaviour._arCameraComponent = _gameObject.AddComponent<Camera>();

            var cameraObject = new GameObject();
            _graffitiWallBehaviour._sketcherCamera = cameraObject.AddComponent<Camera>();
            _graffitiWallBehaviour._sketcherCamera.enabled = false;

            var hudCanvas = _gameObject.AddComponent<Canvas>();
            hudCanvas.enabled = true;
            _graffitiWallBehaviour._hudCanvas = hudCanvas;
        }

        [Test]
        public void Update_OnTouchSketcherCameraEnabledAndHudDisabled()
        {
            var touch = new Touch
                {position = new Vector2(4, 4), deltaPosition = new Vector2(2, 2), phase = TouchPhase.Began};
            _graffitiWallBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _graffitiWallBehaviour.physicsHandler =
                MockPhysicsHandler<GraffitiWallBehaviour>.ReturnsDetected(_graffitiWallBehaviour);

            _graffitiWallBehaviour.Update();

            Assert.IsTrue(_graffitiWallBehaviour._sketcherCamera.enabled);
            Assert.IsFalse(_graffitiWallBehaviour._hudCanvas.enabled);
        }

        [Test]
        public void Update_OnNoTouchSketcherCameraDoesNotEnable()
        {
            var touch = new Touch
                {position = new Vector2(4, 4), deltaPosition = new Vector2(2, 2), phase = TouchPhase.Began};
            _graffitiWallBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _graffitiWallBehaviour.physicsHandler = new MockPhysicsHandler<GraffitiWallBehaviour>();

            _graffitiWallBehaviour.Update();

            Assert.IsFalse(_graffitiWallBehaviour._sketcherCamera.enabled);
        }
    }
}
