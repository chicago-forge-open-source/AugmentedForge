using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Markers;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Tests.EditMode.Markers
{
    public class MarkerControlBehaviourTests
    {
        private GameObject _markerGameObject;
        private MarkerControlBehaviour _markerBehaviour;

        [SetUp]
        public void Setup()
        {
            _markerGameObject = new GameObject();
            _markerBehaviour = _markerGameObject.AddComponent<MarkerControlBehaviour>();
            _markerBehaviour.ArCameraGameObject = new GameObject();
        }

        [Test]
        public void Start_WillAddBehavioursWithCorrectEnabledState()
        {
            _markerBehaviour.Start();

            var markerFaceCameraBehaviour = _markerGameObject.GetComponent<MarkerFaceCameraBehaviour>();
            Assert.IsTrue(markerFaceCameraBehaviour.enabled);
            Assert.AreEqual(_markerBehaviour.ArCameraGameObject, markerFaceCameraBehaviour.ArCameraGameObject);
            Assert.IsFalse(_markerGameObject.GetComponent<MarkerSpinBehaviour>().enabled);
        }

        [Test]
        public void Update_WhenMarkerIsActiveWillEnableSpinBehaviourAndDisableFaceCameraBehaviour()
        {
            _markerBehaviour.Marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0) {Active = true};
            _markerBehaviour.InputHandler = new MockInputHandler(new List<Touch>());
            _markerBehaviour.PhysicsHandler = new MockPhysicsHandler<MarkerControlBehaviour>();

            _markerBehaviour.Start();

            _markerBehaviour.Update();

            var markerSpinBehaviour = _markerGameObject.GetComponent<MarkerSpinBehaviour>();

            Assert.IsTrue(markerSpinBehaviour.enabled);
            Assert.IsFalse(_markerGameObject.GetComponent<MarkerFaceCameraBehaviour>().enabled);
        }

        [Test]
        public void Update_WhenMarkerIsNotActiveThenDisableSpinBehaviourAndEnableFaceCameraBehaviour()
        {
            _markerBehaviour.Marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0);
            _markerBehaviour.InputHandler = new MockInputHandler(new List<Touch>());
            _markerBehaviour.PhysicsHandler = new MockPhysicsHandler<MarkerControlBehaviour>();
            _markerBehaviour.Start();

            _markerBehaviour.Update();

            Assert.IsFalse(_markerGameObject.GetComponent<MarkerSpinBehaviour>().enabled);

            var markerFaceCameraBehaviour = _markerGameObject.GetComponent<MarkerFaceCameraBehaviour>();
            Assert.IsTrue(markerFaceCameraBehaviour.enabled);
        }

        [Test]
        public void Update_WhenTouchIsDetectedSetMarkerActive()
        {
            var touch = new Touch {position = new Vector2(4, 4), deltaPosition = new Vector2(2, 2)};
            _markerBehaviour.InputHandler = new MockInputHandler(new List<Touch> {touch});

            var uniqueIdentifier = "Aqua-pup Castle";
            _markerGameObject.name = uniqueIdentifier;
            _markerBehaviour.PhysicsHandler = PhysicsHandlerThatReturnsDetected();
            _markerBehaviour.Marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0);

            _markerBehaviour.Start();

            _markerBehaviour.Update();

            Assert.IsTrue(_markerBehaviour.Marker.Active);
        }

        [Test]
        public void Update_WhenTouchIsNotDetectedMarkerIsUnaffected()
        {
            _markerBehaviour.Marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0);
            _markerBehaviour.InputHandler = new MockInputHandler(new List<Touch>());
            _markerBehaviour.PhysicsHandler = new MockPhysicsHandler<MarkerControlBehaviour>();

            _markerBehaviour.Start();

            _markerBehaviour.Update();

            Assert.IsFalse(_markerBehaviour.Marker.Active);
        }

        private MockPhysicsHandler<MarkerControlBehaviour> PhysicsHandlerThatReturnsDetected()
        {
            return new MockPhysicsHandler<MarkerControlBehaviour>
                {ValueToReturn = _markerBehaviour};
        }
    }

    public class MockPhysicsHandler<RT> : PhysicsHandler where RT : class
    {
        public RT ValueToReturn { get; set; }

        public T Raycast<T>(Ray ray) where T : class
        {
            Debug.Log(ray);
            return ValueToReturn as T;
        }
    }
}