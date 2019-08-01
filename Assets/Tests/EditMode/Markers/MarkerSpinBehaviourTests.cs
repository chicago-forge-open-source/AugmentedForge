using Markers;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Assets.Tests.EditMode.Markers
{
    public class MarkerSpinBehaviourTests
    {
        private const int FramesPerSecond = 30;
        private const int ExpectedRotationAmount = 360 / FramesPerSecond;
        private GameObject _markerGameObject;
        private MarkerSpinBehaviour _markerBehaviour;
        private readonly QuaternionEqualityComparer _quaternionComparer = new QuaternionEqualityComparer(10e-6f);

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

            Assert.That(_markerGameObject.transform.rotation,
                Is.EqualTo(Quaternion.Euler(0, ExpectedRotationAmount, 0)).Using(_quaternionComparer)
            );
        }

        [Test]
        public void Update_WillAddRotationToMarker()
        {
            var initialRotation = 90;
            _markerBehaviour.marker = new Marker("", 0, 0) {Active = true};
            _markerGameObject.transform.rotation = Quaternion.Euler(0, initialRotation, 0);

            _markerBehaviour.Update();

            Assert.That(_markerGameObject.transform.rotation,
                Is.EqualTo(Quaternion.Euler(0, initialRotation + ExpectedRotationAmount, 0)).Using(_quaternionComparer)
            );
        }

        [Test]
        public void Update_WhenMarkersIsNotActiveThenNoRotation()
        {
            _markerBehaviour.marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0);

            _markerBehaviour.Update();

            Assert.That(_markerGameObject.transform.rotation,
                Is.EqualTo(Quaternion.Euler(0, 0, 0)).Using(_quaternionComparer)
            );
        }

        [Test]
        public void Update_WhenMarkerMakesFullRotationThenMarkerBecomesInactive()
        {
            var initialRotation = 78;
            _markerBehaviour.marker = new Marker("", 0, 0) {Active = true};
            var rotation = Quaternion.Euler(0, initialRotation, 0);
            _markerGameObject.transform.rotation = rotation;
            _markerBehaviour.rotatedFullCircle = false;
            _markerBehaviour.rotationCount = 29;

            _markerBehaviour.Update();
            
            Assert.That(_markerGameObject.transform.rotation,
                Is.EqualTo(Quaternion.Euler(0, initialRotation + ExpectedRotationAmount, 0)).Using(_quaternionComparer)
            );
            Assert.IsTrue(_markerBehaviour.rotatedFullCircle);
        }
    }
}