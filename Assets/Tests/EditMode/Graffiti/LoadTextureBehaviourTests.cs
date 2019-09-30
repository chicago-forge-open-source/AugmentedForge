using System.IO;
using System.Linq;
using System.Text;
using Graffiti;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Graffiti
{
    public class LoadTextureBehaviourTests
    {
        private LoadTextureBehaviour _loadTextureBehaviour;
        private TextureBehaviour _textureBehaviour;

        [SetUp]
        public void SetUp()
        {
            var gameObject = new GameObject();
            _loadTextureBehaviour = gameObject.AddComponent<LoadTextureBehaviour>();
            _textureBehaviour = gameObject.AddComponent<TextureBehaviour>();
            _loadTextureBehaviour.textureBehaviour = _textureBehaviour;
        }

        [Test]
        public void Start_GivenNoSavedTexture_TextureIsAllBlack()
        {
            File.Delete(Application.persistentDataPath + "/SavedImage.csv");

            _loadTextureBehaviour.Start();

            Assert.IsFalse(_textureBehaviour.LitPoints.Any());
        }
        
        [Test]
        public void Start_GivenSavedTexture_TextureIsLoaded()
        {
            File.WriteAllBytes(Application.persistentDataPath + "/SavedImage.csv",
                Encoding.UTF8.GetBytes("25,75\n85,10\n")
            );

            _loadTextureBehaviour.Start();

            Assert.AreEqual(new Vector2(25, 75), _textureBehaviour.LitPoints[0]);
            Assert.AreEqual(new Vector2(85, 10), _textureBehaviour.LitPoints[1]);
        }
    }
}