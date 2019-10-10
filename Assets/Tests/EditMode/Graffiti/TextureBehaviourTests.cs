using Graffiti;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tests.Mocks;

namespace Tests.EditMode.Graffiti
{
    public class TextureBehaviourTests
    {
        private GameObject _gameObject;
        private TextureBehaviour _behaviour;
        private SketcherInputBehaviour _inputBehaviour;

        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _behaviour = _gameObject.AddComponent<TextureBehaviour>();
            _behaviour.material = new Material(Shader.Find(" Diffuse"));
            _inputBehaviour = _gameObject.AddComponent<SketcherInputBehaviour>();
            _inputBehaviour.sketcherTextureBehaviour = _behaviour;
            var sketcherCameraGameObject = new GameObject();
            _inputBehaviour.sketcherCamera = sketcherCameraGameObject.AddComponent<Camera>();

            _inputBehaviour.touchDetector = new UnityPlaneTouchDetector
            {
                inputHandler = new MockInputHandler(new List<Touch>()),
                physicsHandler = new MockPhysicsHandler<TextureBehaviour>()
            };
        }

        [TearDown]
        public void TearDown()
        {
            var pngFilePath = Application.persistentDataPath + "/texture_000.png";
            if (File.Exists(pngFilePath))
            {
                File.Delete(pngFilePath);
            }
        }

        [Test]
        public void Start_GivenNoSavedTexture_TextureIsAllBlack()
        {
            _behaviour.Start();

            var mainTexture = GetMainTexture();

            for (var y = 0; y < mainTexture.height; y++)
            {
                for (var x = 0; x < mainTexture.width; x++)
                {
                    Assert.AreEqual(mainTexture.GetPixel(x, y), Color.black);
                }
            }

            Assert.AreEqual(50, mainTexture.width);
            Assert.AreEqual(50, mainTexture.height);
        }

        [Test]
        public void Start_GivenSavedTexture_TextureIsLoaded()
        {
            _behaviour.LitPoints.Add(new Vector2(25, 75));
            _behaviour.LitPoints.Add(new Vector2(85, 10));

            _behaviour.Start();

            var mainTexture = GetMainTexture();

            Assert.AreEqual(mainTexture.GetPixel(25, 75), Color.white);
            Assert.AreEqual(mainTexture.GetPixel(85, 10), Color.white);
        }

        [Test]
        public void UntouchedBehaviourAppliesAllBlackTexture()
        {
            _behaviour.Update();

            var mainTexture = GetMainTexture();

            for (var y = 0; y < mainTexture.height; y++)
            {
                for (var x = 0; x < mainTexture.width; x++)
                {
                    Assert.AreEqual(mainTexture.GetPixel(x, y), Color.black);
                }
            }

            Assert.AreEqual(50, mainTexture.width);
            Assert.AreEqual(50, mainTexture.height);
        }

        [Test]
        public void Update_OnTouchColorChangesToWhiteAtTouchLocation()
        {
            _behaviour.transform.position = new Vector3(50, 50, 50);
            _behaviour.transform.localScale = new Vector3(2f, 5f, 2f);

            var touch = new Touch {position = new Vector2(4, 4)};
            _inputBehaviour.touchDetector.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _inputBehaviour.touchDetector.physicsHandler = new MockPhysicsHandler<TextureBehaviour>
            {
                ValueToReturn = _behaviour, HitPointToReturn = new Vector3(0, 40f, 40f)
            };


            _inputBehaviour.Update();
            _behaviour.Update();

            var mainTexture = GetMainTexture();

            Assert.AreEqual(mainTexture.GetPixel(50, 0), Color.white);
        }

        [Test]
        public void Update_MultipleTouchesMakeMultipleWhitePixels()
        {
            _behaviour.transform.position = new Vector3(50, 50, 50);
            _behaviour.transform.localScale = new Vector3(2f, 5f, 2f);

            TouchAndUpdate(45, 55);
            TouchAndUpdate(55, 45);
            TouchAndUpdate(60, 59.5f);

            var mainTexture = GetMainTexture();

            Assert.AreEqual(mainTexture.GetPixel(38, 38), Color.white);
            Assert.AreEqual(mainTexture.GetPixel(12, 12), Color.white);
            Assert.AreEqual(mainTexture.GetPixel(0, 49), Color.white);
        }

        [Test]
        public void SaveTextureToPng()
        {
            _behaviour.Start();
            _behaviour.SaveAsPng();
            Assert.IsTrue(File.Exists(Application.persistentDataPath + "/texture_000.png"));
        }

        [Test]
        public void SaveTextToPngSavesBytesFromTexture()
        {
            _behaviour.Start();
            _behaviour.SaveAsPng();
            var data = File.ReadAllBytes(Application.persistentDataPath + "/texture_000.png");
            var mainTextureAs2D = ((Texture2D) _behaviour.material.mainTexture);
            Assert.IsTrue(data.SequenceEqual(mainTextureAs2D.EncodeToPNG()));
        }

        private void TouchAndUpdate(float gameSpaceZ, float gameSpaceY)
        {
            var touch = new Touch {position = new Vector2(gameSpaceZ, gameSpaceY)};
            _inputBehaviour.touchDetector.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _inputBehaviour.touchDetector.physicsHandler = new MockPhysicsHandler<TextureBehaviour>
            {
                ValueToReturn = _behaviour, HitPointToReturn = new Vector3(0, gameSpaceY, gameSpaceZ)
            };

            _inputBehaviour.Update();
            _behaviour.Update();
        }

        private Texture2D GetMainTexture()
        {
            var mainTexture = _behaviour.material.mainTexture as Texture2D;
            Assert.IsNotNull(mainTexture);
            return mainTexture;
        }
    }
}