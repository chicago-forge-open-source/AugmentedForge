using System;
using AR;
using DataLoaders;
using DefaultNamespace;
using NUnit.Framework;
using SyncPoints;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Tests.EditMode
{
    public class ArCalibrationBehaviourTests
    {
        private GameObject _game;
        private ArCalibrationBehaviour _mapScript;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _game.AddComponent<SpriteRenderer>();
            _mapScript = _game.AddComponent<ArCalibrationBehaviour>();
            _mapScript.debugText = _game.AddComponent<Text>();

            _mapScript.arCameraGameObject = new GameObject();
            _mapScript.arCameraGameObject.AddComponent<ARCameraBackground>();
            _mapScript.arCameraGameObject.GetComponent<Camera>().cullingMask = 567;

            _mapScript.startPoint = new GameObject();
            _mapScript.arSessionOrigin = new GameObject();
            _mapScript.arSessionOrigin.AddComponent<ARSessionOrigin>();
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
        public void Start_GivenALocation_ExpectedSyncPointIsLoaded()
        {
            new ChicagoDataLoader().DataLoad();
            var expectedSyncPointPosition = new Vector3(0, 0, -1.5f);
            PlayerSelections.startingParametersProvided = false;
            Repositories.SyncPointRepository.Save(new[]
                {new SyncPoint("test", expectedSyncPointPosition.x, expectedSyncPointPosition.z, 0)});
            _mapScript.Start();

            var actualSyncPointPosition = _mapScript.startPoint.transform.position;
            //TODO: Move to Vector3 Comparer (may want to add to TestHelpers class now and update other references)
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.x - actualSyncPointPosition.x) < .1);
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.z - actualSyncPointPosition.z) < .01);
        }

        [Test]
        public void GivenAPlayerStartPointIsProvided_SyncPointIsSetToPlayerStartParameters()
        {
            new ChicagoDataLoader().DataLoad();
            var expectedPosition = new Vector3(1, 0, 1);
            PlayerSelections.startingPoint = expectedPosition;
            var expectedYRotation = 90f;
            PlayerSelections.orientation = expectedYRotation;
            PlayerSelections.startingParametersProvided = true;

            _mapScript.Start();

            var actualPosition = _mapScript.startPoint.transform.position;
            Assert.AreEqual(expectedPosition.x, actualPosition.x);
            Assert.AreEqual(expectedPosition.z, actualPosition.z);
            TestHelpers.AssertQuaternionsAreEqual(
                Quaternion.Euler(0, expectedYRotation, 0),
                _mapScript.arSessionOrigin.transform.rotation);
        }
    }
}