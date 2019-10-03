using System.IO;
using System.Text;
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
        private MockPlaneTouchDetector _mockPlaneTouchDetector;

        [SetUp]
        public void SetUp()
        {
            var graffitiWall = new GameObject();
            _dropInputBehaviour = graffitiWall.AddComponent<DropGraffitiInputBehaviour>();
            _graffitiTextureBehaviour = graffitiWall.AddComponent<TextureBehaviour>();
            _graffitiTextureBehaviour.material = new Material(Shader.Find(" Diffuse"));
            _graffitiTextureBehaviour.textureSize = 10000;
            _mockPlaneTouchDetector = new MockPlaneTouchDetector();
            _dropInputBehaviour.planeTouchDetector = _mockPlaneTouchDetector;

            var sketcher = new GameObject();
            _sketcherTextureBehaviour = sketcher.AddComponent<TextureBehaviour>();
            _sketcherTextureBehaviour.textureSize = 50;

            _dropInputBehaviour.graffitiTextureBehaviour = _graffitiTextureBehaviour;
            _dropInputBehaviour.sketcherTextureBehaviour = _sketcherTextureBehaviour;
            _dropInputBehaviour.sketcherCamera = new GameObject().AddComponent<Camera>();
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
        public void OnEnable_WillSetDropPointToMiddleOfScreen()
        {
            _dropInputBehaviour.dropPoint = new Vector2(99, 99);
            _dropInputBehaviour.OnEnable();

            var halfway = _graffitiTextureBehaviour.textureSize / 2;
            Assert.AreEqual(new Vector2(halfway, halfway), _dropInputBehaviour.dropPoint);
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

        [Test]
        public void Update_Repeatedly_DoesNotLeaveTrail()
        {
            _graffitiTextureBehaviour.LitPoints.Add(new Vector2(99, 99));
            _sketcherTextureBehaviour.LitPoints.Add(new Vector2(1, 2));

            _dropInputBehaviour.OnEnable();

            _dropInputBehaviour.dropPoint = new Vector2(10, 10);
            _dropInputBehaviour.Update();

            _dropInputBehaviour.dropPoint = new Vector2(100, 100);
            _dropInputBehaviour.Update();

            Assert.AreEqual(2, _graffitiTextureBehaviour.LitPoints.Count);
            Assert.AreEqual(new Vector2(99, 99), _graffitiTextureBehaviour.LitPoints[0]);
            Assert.AreEqual(new Vector2(1, 2) + _dropInputBehaviour.dropPoint, _graffitiTextureBehaviour.LitPoints[1]);
        }

        [Test]
        public void Update_WillMoveDropPointWhenPlaneTouchDetectorSaysSo()
        {
            var expectedPoint = new Vector2(19, 99);
            _mockPlaneTouchDetector.PointToReturn = expectedPoint;
            _dropInputBehaviour.dropPoint = new Vector2(20, 30);
            _dropInputBehaviour.Update();

            Assert.AreEqual(expectedPoint, _dropInputBehaviour.dropPoint);
            Assert.AreEqual(_dropInputBehaviour.gameObject.transform, _mockPlaneTouchDetector.lastTransform);
            Assert.AreEqual(_dropInputBehaviour.sketcherCamera, _mockPlaneTouchDetector.lastCamera);
            Assert.AreEqual(_graffitiTextureBehaviour.textureSize, _mockPlaneTouchDetector.lastTextureSize);
        }

        [Test]
        public void Save_WillSaveToFile()
        {
            _graffitiTextureBehaviour.Start();
            _dropInputBehaviour.graffitiTextureBehaviour.LitPoints.Add(new Vector2(0, 49));

            _dropInputBehaviour.SaveBits();

            var rawBytes = File.ReadAllBytes(Application.persistentDataPath + "/SavedImage.csv");
            var fileContent = Encoding.UTF8.GetString(rawBytes);

            Assert.AreEqual("0,49\n", fileContent);
        }

        [Test]
        public void Save_MultipleSavesTheLastOneWillWin()
        {
            _graffitiTextureBehaviour.Start();
            _dropInputBehaviour.graffitiTextureBehaviour.LitPoints.Add(new Vector2(0, 49));
            _dropInputBehaviour.SaveBits();

            _dropInputBehaviour.graffitiTextureBehaviour.LitPoints.Clear();
            _dropInputBehaviour.graffitiTextureBehaviour.LitPoints.Add(new Vector2(50, 0));

            _dropInputBehaviour.SaveBits();

            var rawBytes = File.ReadAllBytes(Application.persistentDataPath + "/SavedImage.csv");
            var fileContent = Encoding.UTF8.GetString(rawBytes);

            Assert.AreEqual("50,0\n", fileContent);
            Assert.IsTrue(File.Exists(Application.persistentDataPath + "/texture_000.png"));
        }
    }

    public class MockPlaneTouchDetector : PlaneTouchDetector
    {
        public Transform lastTransform;
        public Camera lastCamera;
        public int lastTextureSize;

        public Vector2? FindTouchedPoint(Transform transform, Camera camera, int textureSize)
        {
            lastTextureSize = textureSize;
            lastCamera = camera;
            lastTransform = transform;
            return PointToReturn;
        }

        public Vector2? PointToReturn { get; set; }
    }
}