using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Tests.EditMode
{
    public class ArViewEditTests
    {
        private GameObject _game;
        private readonly QuaternionEqualityComparer _quaternionComparer = new QuaternionEqualityComparer(10e-6f);
        private ARView _mapScript;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _game.AddComponent<SpriteRenderer>();
            _mapScript = _game.AddComponent<ARView>();
            _mapScript.debugText = _game.AddComponent<Text>();

            _mapScript.arCameraGameObject = new GameObject();
            _mapScript.arCameraGameObject.AddComponent<ARCameraBackground>();
            _mapScript.arCameraGameObject.GetComponent<Camera>().cullingMask = 567;

            _mapScript.startPoint = new GameObject();
            _mapScript.arSessionOrigin = new GameObject();
        }

        [Test]
        public void Start_WillMoveArOriginToStartPoint()
        {
            _mapScript.Start();

            var position = _mapScript.arSessionOrigin.transform.position;
            Assert.AreEqual(_mapScript.startPoint.transform.position, position);
        }

        [Test]
        public void Start_WillUpdateArSessionOriginRotationBasedOnCompass()
        {
            _mapScript.compass = new MockCompass {TrueHeading = 180f};

            _mapScript.Start();

            TestHelpers.AssertQuaternionsAreEqual(
                Quaternion.Euler(0, 180f, 0),
                _mapScript.arSessionOrigin.transform.rotation
            );
        }
        
        [Test]
        public void Start_GivenChicagoAsTheLocation_ChicagoSyncPointIsLoaded()
        {
            PlayerPrefs.SetString("location", "Chicago");
            
            _mapScript.Start();
         
            var expectedSyncPointPosition = new Vector3(26.94955f, 0, -18.17933f);
            var actualSyncPointPosition = _mapScript.startPoint.transform.position;
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.x - actualSyncPointPosition.x) < .1);
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.y - actualSyncPointPosition.y) < .1);
        }
        
        [Test]
        public void Start_GivenIowaAsTheLocation_IowaSyncPointIsLoaded()
        {
            PlayerPrefs.SetString("location", "Iowa");
            
            _mapScript.Start();
            
            var expectedSyncPointPosition = new Vector3(0, 0, -18.17933f);
            var actualSyncPointPosition = _mapScript.startPoint.transform.position;
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.x - actualSyncPointPosition.x) < .1);
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.y - actualSyncPointPosition.y) < .01);
        }

    }
}