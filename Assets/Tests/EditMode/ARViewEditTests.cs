using System;
using Assets.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Assets.Tests.EditMode
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
            _mapScript.DebugText = _game.AddComponent<Text>();

            _mapScript.ArCameraComponent = new GameObject();
            _mapScript.ArCameraComponent.AddComponent<ARCameraBackground>();
            _mapScript.ArCameraComponent.GetComponent<Camera>().cullingMask = 567;

            _mapScript.StartPoint = new GameObject();
            _mapScript.ArSessionOrigin = new GameObject();
        }

        [Test]
        public void Start_WillMoveArOriginToStartPoint()
        {
            _mapScript.Start();

            var position = _mapScript.ArSessionOrigin.transform.position;
            Assert.AreEqual(_mapScript.StartPoint.transform.position, position);
        }

        [Test]
        public void Start_WillUpdateArSessionOriginRotationBasedOnCompass()
        {
            _mapScript.Compass = new MockCompass {TrueHeading = 180f};

            _mapScript.Start();

            var expectedCameraRotation = Quaternion.Euler(0, 180f, 0);

            Assert.That(_mapScript.ArSessionOrigin.transform.rotation,
                Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer)
            );
        }
        
        [Test]
        public void Start_GivenChicagoAsTheLocation_ChicagoSyncPointIsLoaded()
        {
            PlayerPrefs.SetString("location", "Chicago");
            
            _mapScript.Start();
         
            var expectedSyncPointPosition = new Vector3(26.94955f, 0, -18.17933f);
            var actualSyncPointPosition = _mapScript.StartPoint.transform.position;
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.x - actualSyncPointPosition.x) < .1);
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.y - actualSyncPointPosition.y) < .1);
        }
        
        [Test]
        public void Start_GivenIowaAsTheLocation_IowaSyncPointIsLoaded()
        {
            PlayerPrefs.SetString("location", "Iowa");
            
            _mapScript.Start();
            
            var expectedSyncPointPosition = new Vector3(0, 0, -18.17933f);
            var actualSyncPointPosition = _mapScript.StartPoint.transform.position;
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.x - actualSyncPointPosition.x) < .1);
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.y - actualSyncPointPosition.y) < .01);
        }

    }
}