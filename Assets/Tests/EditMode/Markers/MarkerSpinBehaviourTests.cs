using System;
using Markers;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Markers
{
    public class MarkerSpinBehaviourTests
    {
        private const double SlowdownScale = 2.2;
        private const int FramesPerSecond = 30;
        private static readonly int ExpectedRotationAmount = (int) Math.Round(360 / FramesPerSecond / SlowdownScale);
        private GameObject _markerGameObject;
        private MarkerSpinBehaviour _markerBehaviour;

        [SetUp]
        public void Setup()
        {
            _markerGameObject = new GameObject();
            _markerBehaviour = _markerGameObject.AddComponent<MarkerSpinBehaviour>();
        }

        [Test]
        public void Update_WillRotateMarkerAroundYAxis()
        {
            _markerBehaviour.marker = new Marker("", 0, 0) {Active = true};

            _markerBehaviour.Update();

            TestHelpers.AssertQuaternionsAreEqual(
                Quaternion.Euler(0, ExpectedRotationAmount, 0),
                _markerGameObject.transform.rotation
            );
        }

        [Test]
        public void Update_WillAddRotationToMarker()
        {
            const int initialRotation = 90;
            _markerBehaviour.marker = new Marker("", 0, 0) {Active = true};
            _markerGameObject.transform.rotation = Quaternion.Euler(0, initialRotation, 0);

            _markerBehaviour.Update();


            TestHelpers.AssertQuaternionsAreEqual(
                Quaternion.Euler(0, initialRotation + ExpectedRotationAmount, 0),
                _markerGameObject.transform.rotation
            );
        }

        [Test]
        public void Update_WhenMarkersIsNotActiveThenNoRotation()
        {
            _markerBehaviour.marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0);

            _markerBehaviour.Update();

            TestHelpers.AssertQuaternionsAreEqual(
                Quaternion.Euler(0, 0, 0),
                _markerGameObject.transform.rotation
            );
        }

        [Test]
        public void Update_WhenMarkerMakesFullRotationThenMarkerBecomesInactive()
        {
            const int initialRotation = 78;
            _markerBehaviour.marker = new Marker("", 0, 0) {Active = true};
            var rotation = Quaternion.Euler(0, initialRotation, 0);
            _markerGameObject.transform.rotation = rotation;
            _markerBehaviour.rotatedFullCircle = false;
            _markerBehaviour.rotationCount = 65;

            _markerBehaviour.Update();

            TestHelpers.AssertQuaternionsAreEqual(
                Quaternion.Euler(0, initialRotation + ExpectedRotationAmount, 0),
                _markerGameObject.transform.rotation
            );
            Assert.IsTrue(_markerBehaviour.rotatedFullCircle);
        }
    }
}