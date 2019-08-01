using System.Collections.Generic;
using Assets.Scripts;
using Markers;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Tests.EditMode.Markers
{
    public class MarkerControlBehaviourTests
    {
        private GameObject _markerGameObject;
        private MarkerControlBehaviour _controlBehaviour;

        [SetUp]
        public void Setup()
        {
            _markerGameObject = new GameObject();
            _controlBehaviour = _markerGameObject.AddComponent<MarkerControlBehaviour>();
            _controlBehaviour.arCameraGameObject = new GameObject();
            _controlBehaviour.arCameraGameObject.AddComponent<Camera>();
        }

        [Test]
        public void Start_WillAddBehavioursWithCorrectEnabledState()
        {
            _controlBehaviour.Start();

            var markerFaceCameraBehaviour = _markerGameObject.GetComponent<MarkerFaceCameraBehaviour>();
            Assert.IsTrue(markerFaceCameraBehaviour.enabled);
            Assert.AreEqual(_controlBehaviour.arCameraGameObject, markerFaceCameraBehaviour.arCameraGameObject);
            
            var markerSpinBehaviour = _markerGameObject.GetComponent<MarkerSpinBehaviour>();
            Assert.IsFalse(markerSpinBehaviour.enabled);
            Assert.AreEqual(_controlBehaviour.marker, markerSpinBehaviour.marker);
            
            var markerDistanceBehaviour = _markerGameObject.GetComponent<MarkerDistanceBehaviour>();
            Assert.AreEqual(_controlBehaviour.arCameraGameObject, markerDistanceBehaviour.arCameraGameObject);
        }

        [Test]
        public void Update_WhenMarkerIsActiveWillEnableSpinBehaviourAndDisableFaceCameraBehaviour()
        {
            _controlBehaviour.marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0) {Active = true};
            _controlBehaviour.inputHandler = new MockInputHandler(new List<Touch>());
            _controlBehaviour.physicsHandler = new MockPhysicsHandler<MarkerControlBehaviour>();

            _controlBehaviour.Start();

            _controlBehaviour.Update();

            var markerSpinBehaviour = _markerGameObject.GetComponent<MarkerSpinBehaviour>();

            Assert.IsTrue(markerSpinBehaviour.enabled);
            Assert.IsFalse(_markerGameObject.GetComponent<MarkerFaceCameraBehaviour>().enabled);
        }

        [Test]
        public void Update_WhenMarkerIsNotActiveThenDisableSpinBehaviourAndEnableFaceCameraBehaviour()
        {
            _controlBehaviour.marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0);
            _controlBehaviour.inputHandler = new MockInputHandler(new List<Touch>());
            _controlBehaviour.physicsHandler = new MockPhysicsHandler<MarkerControlBehaviour>();
            _controlBehaviour.Start();
            
            _controlBehaviour.Update();

            Assert.IsFalse(_markerGameObject.GetComponent<MarkerSpinBehaviour>().enabled);

            var markerFaceCameraBehaviour = _markerGameObject.GetComponent<MarkerFaceCameraBehaviour>();
            Assert.IsTrue(markerFaceCameraBehaviour.enabled);
        }

        [Test]
        public void Update_WhenTouchOnMarkerIsDetectedSetMarkerActive()
        {
            var touch = new Touch
                {position = new Vector2(4, 4), deltaPosition = new Vector2(2, 2), phase = TouchPhase.Began};
            _controlBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});

            var uniqueIdentifier = "Aqua-pup Castle";
            _markerGameObject.name = uniqueIdentifier;
            _controlBehaviour.physicsHandler = PhysicsHandlerThatReturnsDetected();
            _controlBehaviour.marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0);

            _controlBehaviour.Start();

            _controlBehaviour.Update();

            Assert.IsTrue(_controlBehaviour.marker.Active);
        }

        [Test]
        public void Update_WhenTouchOnDifferentMarkerIsDetectedThisMarkerIsUnaffected()
        {
            _controlBehaviour.marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0);
            var touch = new Touch
                {position = new Vector2(4, 4), deltaPosition = new Vector2(2, 2), phase = TouchPhase.Began};
            _controlBehaviour.inputHandler = new MockInputHandler(new List<Touch>{touch});
            _controlBehaviour.physicsHandler = new MockPhysicsHandler<MarkerControlBehaviour>();
            var mockPhysicsHandler = PhysicsHandlerReturnsDifferentMarkerDetected();
            _controlBehaviour.physicsHandler = mockPhysicsHandler;
            _controlBehaviour.Start();

            _controlBehaviour.Update();

            Assert.IsFalse(_controlBehaviour.marker.Active);
        }

        [Test]
        public void Update_WhenMarkersSpinsAFulLCircleThenMarkerBecomesInactive()
        {
            _controlBehaviour.inputHandler = new MockInputHandler(new List<Touch>());
            
            _controlBehaviour.Start();
            
            var spinBehaviour = _markerGameObject.GetComponent<MarkerSpinBehaviour>();
            spinBehaviour.rotatedFullCircle = true;
            _controlBehaviour.marker = new Marker("", 0, 0 ) {Active = true};

            _controlBehaviour.Update();

            Assert.IsFalse(_controlBehaviour.marker.Active);
            Assert.IsTrue(_markerGameObject.GetComponent<MarkerFaceCameraBehaviour>().enabled);
            Assert.IsFalse(_markerGameObject.GetComponent<MarkerSpinBehaviour>().enabled);
        }

        private static MockPhysicsHandler<MarkerControlBehaviour> PhysicsHandlerReturnsDifferentMarkerDetected()
        {
            return new MockPhysicsHandler<MarkerControlBehaviour>
            {
                ValueToReturn = new GameObject().AddComponent<MarkerControlBehaviour>()
            };
        }

        private MockPhysicsHandler<MarkerControlBehaviour> PhysicsHandlerThatReturnsDetected()
        {
            return new MockPhysicsHandler<MarkerControlBehaviour>
            {
                ValueToReturn = _controlBehaviour
            };
        }
    }

    public class MockPhysicsHandler<RT> : PhysicsHandler where RT : class
    {
        public RT ValueToReturn { get; set; }

        public T Raycast<T>(Ray ray) where T : class
        {
            return ValueToReturn as T;
        }
    }
}