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
        private ArCalibrationBehaviour _behaviour;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _game.AddComponent<SpriteRenderer>();
            _behaviour = _game.AddComponent<ArCalibrationBehaviour>();
            _behaviour.debugText = _game.AddComponent<Text>();

            _behaviour.arCameraGameObject = new GameObject();
            _behaviour.arCameraGameObject.AddComponent<ARCameraBackground>();
            _behaviour.arCameraGameObject.GetComponent<Camera>().cullingMask = 567;

            _behaviour.arSessionOrigin = new GameObject();
            _behaviour.arSessionOrigin.AddComponent<ARSessionOrigin>();
        }

        [Test]
        public void Start_GivenNoPlayerSelectionsWillScheduleFirstSyncPoint()
        {
            PlayerSelections.startingParametersProvided = false;
            var startSyncPoint = new SyncPoint("test", 10, -3, 90);
            Repositories.SyncPointRepository.Save(new[] {startSyncPoint});
            _behaviour.compass = new MockCompass {TrueHeading = 180f};

            _behaviour.Start();

            var expectedSyncPoint = new SyncPoint(
                "start with compass",
                startSyncPoint.X,
                startSyncPoint.Z,
                _behaviour.compass.TrueHeading
            );

            Assert.AreEqual(expectedSyncPoint, _behaviour.scheduledSyncPoint);
        }

        [Test]
        public void Update_GivenScheduledSyncPointWillMoveArSessionOriginToSyncPoint()
        {
            var expectedSyncPoint = new SyncPoint("test", 10, -3, 90);
            _behaviour.scheduledSyncPoint = expectedSyncPoint;

            _behaviour.Update();

            var arSessionOriginPosition = _behaviour.arSessionOrigin.transform.position;
            var arSessionOriginRotation = _behaviour.arSessionOrigin.transform.rotation.eulerAngles;
            Assert.AreEqual(expectedSyncPoint.X, arSessionOriginPosition.x);
            Assert.AreEqual(expectedSyncPoint.Z, arSessionOriginPosition.z);
            Assert.AreEqual(expectedSyncPoint.Orientation, arSessionOriginRotation.y);
            Assert.IsNull(_behaviour.scheduledSyncPoint);
        }

        [Test]
        public void GivenPlayerSelections_SyncPointIsScheduled()
        {
            var expectedPosition = new Vector3(1, 0, 1);
            PlayerSelections.startingPoint = expectedPosition;
            var expectedOrientation = 90f;
            PlayerSelections.orientation = expectedOrientation;
            PlayerSelections.startingParametersProvided = true;

            var expectedSyncPoint = new SyncPoint("QR", expectedPosition.x, expectedPosition.z, expectedOrientation);

            _behaviour.Start();

            Assert.AreEqual(expectedSyncPoint, _behaviour.scheduledSyncPoint);
        }

        [Test]
        public void Update_GivenNoScheduledSyncPoint_ArSessionOriginDoesNotMove()
        {
            _behaviour.scheduledSyncPoint = null;
            var expectedPosition = new Vector3(1, 2, 3);
            _behaviour.arSessionOrigin.transform.position = expectedPosition;
            var expectedRotation = Quaternion.Euler(0, 20, 0);
            _behaviour.arSessionOrigin.transform.rotation = expectedRotation;

            _behaviour.Update();

            Assert.AreEqual(expectedPosition, _behaviour.arSessionOrigin.transform.position);
            TestHelpers.AssertQuaternionsAreEqual(expectedRotation, _behaviour.arSessionOrigin.transform.rotation);
        }
    }
}