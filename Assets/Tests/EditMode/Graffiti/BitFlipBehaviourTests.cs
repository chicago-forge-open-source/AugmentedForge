using Graffiti;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

        private Texture2D GetMainTexture()
        {
            var mainTexture = _behaviour.material.mainTexture as Texture2D;
            Assert.IsNotNull(mainTexture);
            return mainTexture;
        }
    }
}