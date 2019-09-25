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
        }

        [Test]
        public void Update_OnTouchColorChangesToWhiteAtTouchLocation()
        {
            var touch = new Touch {position = new Vector2(4, 4)};
            _behaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});

            _behaviour.Update();

            var mainTexture = GetMainTexture();

            Assert.AreEqual(mainTexture.GetPixel(4, 4), Color.white);
        }

        [Test]
        public void Update_TwoTouchsTwoUpdatesTwoWhitePixels()
        {
            TouchAndUpdate(4,5);
            TouchAndUpdate(3,5);
            
            var mainTexture = GetMainTexture();
            Assert.AreEqual(mainTexture.GetPixel(4, 5), Color.white);
            Assert.AreEqual(mainTexture.GetPixel(3, 5), Color.white);
        }

        private void TouchAndUpdate(int x, int y)
        {
            var touch = new Touch {position = new Vector2(x, y)};
            _behaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});

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