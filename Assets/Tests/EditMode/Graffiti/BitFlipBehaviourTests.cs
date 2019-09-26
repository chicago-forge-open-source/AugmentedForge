using Graffiti;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Tests.Mocks;

namespace Tests.EditMode.Graffiti
{
    public class BitFlipBehaviourTests
    {
        private GameObject _gameObject;
        private BitFlipBehaviour _behaviour;

        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _behaviour = _gameObject.AddComponent<BitFlipBehaviour>();
            var sketcherCameraGameObject = new GameObject();
            _behaviour.sketcherCamera = sketcherCameraGameObject.AddComponent<Camera>();
            _behaviour.material = new Material(Shader.Find(" Diffuse"));
            _behaviour.inputHandler = new MockInputHandler(new List<Touch>());
            _behaviour.physicsHandler = new MockPhysicsHandler<BitFlipBehaviour>();
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
            _behaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _behaviour.physicsHandler = new MockPhysicsHandler<BitFlipBehaviour>
            {
                ValueToReturn = _behaviour, HitPointToReturn = new Vector3(0, 40f, 40f)
            };


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
            _behaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _behaviour.physicsHandler = new MockPhysicsHandler<BitFlipBehaviour>
            {
                ValueToReturn = _behaviour, HitPointToReturn = new Vector3(0, gameSpaceY, gameSpaceZ)
            };

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