using Graffiti;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tests.Mocks;

namespace Tests.EditMode.Graffiti
{
    public class GraffitiTextureBehaviourTests
    {
        private GameObject _gameObject;
        private GraffitiTextureBehaviour _behaviour;
        private SketcherInputBehaviour _inputBehaviour;

        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _behaviour = _gameObject.AddComponent<GraffitiTextureBehaviour>();
            _behaviour.material = new Material(Shader.Find(" Diffuse"));
            _inputBehaviour = _gameObject.AddComponent<SketcherInputBehaviour>();
            _behaviour.sketcherInputBehaviour = _inputBehaviour;
            var sketcherCameraGameObject = new GameObject();
            _inputBehaviour.sketcherCamera = sketcherCameraGameObject.AddComponent<Camera>();
            _inputBehaviour.inputHandler = new MockInputHandler(new List<Touch>());
            _inputBehaviour.physicsHandler = new MockPhysicsHandler<GraffitiTextureBehaviour>();
        }

        [Test]
        public void Start_GivenNoSavedTexture_TextureIsAllBlack()
        {
            File.Delete(Application.persistentDataPath + "/SavedImage.csv");

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
            File.WriteAllBytes(Application.persistentDataPath + "/SavedImage.csv",
                Encoding.UTF8.GetBytes("25,75\n85,10\n")
            );

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
            _inputBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _inputBehaviour.physicsHandler = new MockPhysicsHandler<GraffitiTextureBehaviour>
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

        private void TouchAndUpdate(float gameSpaceZ, float gameSpaceY)
        {
            var touch = new Touch {position = new Vector2(gameSpaceZ, gameSpaceY)};
            _inputBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _inputBehaviour.physicsHandler = new MockPhysicsHandler<GraffitiTextureBehaviour>
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