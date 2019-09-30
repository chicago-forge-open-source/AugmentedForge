using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Graffiti;
using NUnit.Framework;
using Tests.Mocks;
using UnityEngine;

namespace Tests.EditMode.Graffiti
{
    public class GraffitiInputBehaviourTests
    {
        private GameObject _gameObject;
        private SketcherInputBehaviour _behaviour;

        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _behaviour = _gameObject.AddComponent<SketcherInputBehaviour>();
            var sketcherCameraGameObject = new GameObject();
            _behaviour.sketcherCamera = sketcherCameraGameObject.AddComponent<Camera>();
            _behaviour.inputHandler = new MockInputHandler(new List<Touch>());
            _behaviour.physicsHandler = new MockPhysicsHandler<GraffitiTextureBehaviour>();
        }

        [Test]
        public void Save_WillSaveToFile()
        {
            _behaviour.transform.position = new Vector3(50, 50, 50);
            _behaviour.transform.localScale = new Vector3(2f, 5f, 2f);

            TouchAndUpdate(60, 59.5f);

            _behaviour.SaveBits();

            var rawBytes = File.ReadAllBytes(Application.persistentDataPath + "/SavedImage.csv");
            var fileContent = Encoding.UTF8.GetString(rawBytes);

            Assert.AreEqual("0,49\n", fileContent);
        }

        [Test]
        public void Save_MultipleSavesTheLastOneWillWin()
        {
            _behaviour.transform.position = new Vector3(50, 50, 50);
            _behaviour.transform.localScale = new Vector3(2f, 5f, 2f);

            TouchAndUpdate(60, 59.5f);
            _behaviour.SaveBits();
            _behaviour.ClearOnClick();
            TouchAndUpdate(40,40);
            _behaviour.SaveBits();

            var rawBytes = File.ReadAllBytes(Application.persistentDataPath + "/SavedImage.csv");
            var fileContent = Encoding.UTF8.GetString(rawBytes);

            Assert.AreEqual("50,0\n", fileContent);
        }

        [Test]
        public void Update_OnTouchColorAddsToLitPointsAtTouchLocation()
        {
            _behaviour.transform.position = new Vector3(50, 50, 50);
            _behaviour.transform.localScale = new Vector3(2f, 5f, 2f);

            var touch = new Touch {position = new Vector2(4, 4)};
            _behaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _behaviour.physicsHandler = new MockPhysicsHandler<SketcherInputBehaviour>
            {
                ValueToReturn = _behaviour, HitPointToReturn = new Vector3(0, 40f, 40f)
            };


            _behaviour.Update();

            Assert.Contains(new Vector2(50, 0), _behaviour.LitPoints);
        }

        [Test]
        public void OnClickClearWillRemoveAllDots()
        {
            _behaviour.transform.position = new Vector3(50, 50, 50);
            _behaviour.transform.localScale = new Vector3(2f, 5f, 2f);

            TouchAndUpdate(45, 55);
            
            _behaviour.ClearOnClick();
            
            Assert.IsEmpty(_behaviour.LitPoints);
        }

        [Test]
        public void Update_MultipleTouchesMakeMultipleLitPoints()
        {
            _behaviour.transform.position = new Vector3(50, 50, 50);
            _behaviour.transform.localScale = new Vector3(2f, 5f, 2f);

            TouchAndUpdate(45, 55);
            TouchAndUpdate(55, 45);
            TouchAndUpdate(60, 59.5f);

            ContainVector2(new Vector2(37.5f, 37.5f), _behaviour.LitPoints, 0.05);
            ContainVector2(new Vector2(12.5f, 12.5f), _behaviour.LitPoints, 0.05);
            ContainVector2(new Vector2(0.0f, 48.8f), _behaviour.LitPoints, 0.05);
        }

        private void ContainVector2(Vector2 expected, List<Vector2> actualList, double acceptableDelta)
        {
            var resultIndex = actualList.FindIndex(vector =>
            {
                var delta = expected - vector;
                return Math.Abs(delta.x) < acceptableDelta 
                       && Math.Abs(delta.y) < acceptableDelta;
            });

            Assert.AreNotEqual(-1, resultIndex);
        }

        private void TouchAndUpdate(float gameSpaceZ, float gameSpaceY)
        {
            var touch = new Touch {position = new Vector2(gameSpaceZ, gameSpaceY)};
            _behaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            _behaviour.physicsHandler = new MockPhysicsHandler<SketcherInputBehaviour>
            {
                ValueToReturn = _behaviour, HitPointToReturn = new Vector3(0, gameSpaceY, gameSpaceZ)
            };

            _behaviour.Update();
        }
    }
}