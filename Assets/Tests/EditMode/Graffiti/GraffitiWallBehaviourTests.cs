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
            _graffitiWallBehaviour.sketcherInputBehaviour = _gameObject.AddComponent<SketcherInputBehaviour>();
            _graffitiWallBehaviour.arCameraComponent = _gameObject.AddComponent<Camera>();

            var sketcherCameraObject = new GameObject();
            _graffitiWallBehaviour.sketcherCamera = sketcherCameraObject.AddComponent<Camera>();
            _graffitiWallBehaviour.sketcherCamera.enabled = false;
            var sketcherCanvas = _gameObject.AddComponent<Canvas>();
            sketcherCanvas.enabled = false;
            _graffitiWallBehaviour.sketcherUi = sketcherCanvas;
            _graffitiWallBehaviour.sketcherSurface = new GameObject();

            var hudCanvas = new GameObject().AddComponent<Canvas>();
            hudCanvas.enabled = true;
            _graffitiWallBehaviour.hudCanvas = hudCanvas;

            _graffitiWallBehaviour.dropGraffitiInputBehaviour =
                new GameObject().AddComponent<DropGraffitiInputBehaviour>();
            _graffitiWallBehaviour.dropGraffitiUi = new GameObject().AddComponent<Canvas>();
        }

        [Test]
        public void Start_OnTouchSketcherCameraDisabledAndHudEnabled()
        {
            _graffitiWallBehaviour.sketcherCamera.enabled = true;
            _graffitiWallBehaviour.sketcherUi.enabled = true;
            _graffitiWallBehaviour.sketcherSurface.SetActive(true);
            _graffitiWallBehaviour.hudCanvas.enabled = false;

            _graffitiWallBehaviour.dropGraffitiUi.enabled = true;
            _graffitiWallBehaviour.dropGraffitiInputBehaviour.enabled = true;
            _graffitiWallBehaviour.enabled = false;
            
            _graffitiWallBehaviour.Start();

            Assert.IsFalse(_graffitiWallBehaviour.sketcherSurface.activeSelf);
            Assert.IsFalse(_graffitiWallBehaviour.sketcherCamera.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.sketcherUi.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.sketcherInputBehaviour.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.dropGraffitiUi.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.dropGraffitiInputBehaviour.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.hudCanvas.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.gameObject.activeSelf);
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
            _graffitiWallBehaviour.enabled = true;

            _graffitiWallBehaviour.Update();

            Assert.IsTrue(_graffitiWallBehaviour.sketcherSurface.activeSelf);
            Assert.IsTrue(_graffitiWallBehaviour.sketcherCamera.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.sketcherUi.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.sketcherInputBehaviour.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.hudCanvas.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.gameObject.activeSelf);
        }

        [Test]
        public void ReturnToARMode_SketcherCameraDisabledAndHudEnabled()
        {
            _graffitiWallBehaviour.sketcherCamera.enabled = true;
            _graffitiWallBehaviour.sketcherUi.enabled = true;
            _graffitiWallBehaviour.sketcherSurface.SetActive(true);
            _graffitiWallBehaviour.hudCanvas.enabled = false;
            _graffitiWallBehaviour.dropGraffitiUi.enabled = true;
            _graffitiWallBehaviour.dropGraffitiInputBehaviour.enabled = true;
            _graffitiWallBehaviour.enabled = false;

            _graffitiWallBehaviour.ReturnToARMode();

            Assert.IsFalse(_graffitiWallBehaviour.sketcherSurface.activeSelf);
            Assert.IsFalse(_graffitiWallBehaviour.sketcherCamera.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.sketcherUi.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.sketcherInputBehaviour.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.dropGraffitiUi.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.dropGraffitiInputBehaviour.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.hudCanvas.enabled);
        }

        [Test]
        public void SwitchToDropGraffitiMode_EnablesAndDisabledCorrectComponents()
        {
            _graffitiWallBehaviour.sketcherCamera.enabled = false;
            _graffitiWallBehaviour.gameObject.SetActive(false);
            _graffitiWallBehaviour.sketcherSurface.SetActive(true);
            _graffitiWallBehaviour.sketcherUi.enabled = true;
            _graffitiWallBehaviour.dropGraffitiUi.enabled = false;
            _graffitiWallBehaviour.sketcherInputBehaviour.enabled = true;
            _graffitiWallBehaviour.dropGraffitiInputBehaviour.enabled = false;
            _graffitiWallBehaviour.hudCanvas.enabled = true;
            _graffitiWallBehaviour.enabled = true;

            _graffitiWallBehaviour.SwitchToDropGraffitiMode();

            Assert.IsTrue(_graffitiWallBehaviour.sketcherCamera.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.gameObject.activeSelf);
            Assert.IsTrue(_graffitiWallBehaviour.dropGraffitiUi.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.dropGraffitiInputBehaviour.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.sketcherSurface.activeSelf);
            Assert.IsFalse(_graffitiWallBehaviour.sketcherUi.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.sketcherInputBehaviour.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.hudCanvas.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.enabled);
        }

        [Test]
        public void Update_OnNoTouchSketcherCameraDoesNotEnable()
        {
            var touch = new Touch
            {
                position = new Vector2(4, 4),
                deltaPosition = new Vector2(2, 2),
                phase = TouchPhase.Began
            };

            _graffitiWallBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _graffitiWallBehaviour.physicsHandler = new MockPhysicsHandler<GraffitiWallBehaviour>();
            _graffitiWallBehaviour.sketcherInputBehaviour.enabled = false;

            _graffitiWallBehaviour.Update();

            Assert.IsFalse(_graffitiWallBehaviour.sketcherCamera.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.sketcherUi.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.hudCanvas.enabled);
            Assert.IsFalse(_graffitiWallBehaviour.sketcherInputBehaviour.enabled);
            Assert.IsTrue(_graffitiWallBehaviour.gameObject.activeSelf);
        }
    }
}