using Graffiti;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Tests.EditMode.Graffiti
{
    public class DropGraffitiInputBehaviourTests
    {
        private DropGraffitiInputBehaviour _dropInputBehaviour;
        private TextureBehaviour _graffitiTextureBehaviour;
        private TextureBehaviour _sketcherTextureBehaviour;

        [SetUp]
        public void SetUp()
        {
            var graffitiWall = new GameObject();
            _dropInputBehaviour = graffitiWall.AddComponent<DropGraffitiInputBehaviour>();
            _graffitiTextureBehaviour = graffitiWall.AddComponent<TextureBehaviour>();

            var sketcher = new GameObject();
            _sketcherTextureBehaviour = sketcher.AddComponent<TextureBehaviour>();

            _dropInputBehaviour.graffitiTextureBehaviour = _graffitiTextureBehaviour;
            _dropInputBehaviour.sketcherTextureBehaviour = _sketcherTextureBehaviour;
        }

        [Test]
        public void CancelDrop_ResetsGraffitiWallPointsToOriginalState()
        {
            var expectedVector = new Vector2(99, 99);
            _graffitiTextureBehaviour.LitPoints.Add(expectedVector);
            _dropInputBehaviour.OnEnable();
            _graffitiTextureBehaviour.LitPoints.Add(new Vector2(1, 1));

            _dropInputBehaviour.CancelDrop();

            Assert.AreEqual(1, _graffitiTextureBehaviour.LitPoints.Count);
            Assert.AreEqual(expectedVector, _graffitiTextureBehaviour.LitPoints[0]);
        }

        [Test]
        public void Update_AddsSketcherPointsToGraffitiPointsWithOffset()
        {
            _graffitiTextureBehaviour.LitPoints.Add(new Vector2(99, 99));
            _sketcherTextureBehaviour.LitPoints.Add(new Vector2(1, 1));
            _sketcherTextureBehaviour.LitPoints.Add(new Vector2(5, 5));

            _dropInputBehaviour.OnEnable();

            _dropInputBehaviour.dropPoint = new Vector2(20, 30);

            _dropInputBehaviour.Update();

            Assert.AreEqual(3, _graffitiTextureBehaviour.LitPoints.Count);
            Assert.AreEqual(new Vector2(99, 99), _graffitiTextureBehaviour.LitPoints[0]);
            Assert.AreEqual(new Vector2(1, 1) + _dropInputBehaviour.dropPoint, _graffitiTextureBehaviour.LitPoints[1]);
            Assert.AreEqual(new Vector2(5, 5) + _dropInputBehaviour.dropPoint, _graffitiTextureBehaviour.LitPoints[2]);
        }
    }
}