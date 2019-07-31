using System;
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
        private MarkerControlBehaviour _controlBehaviour;

        [SetUp]
        public void Setup()
        {
            _markerGameObject = new GameObject();
            _controlBehaviour = _markerGameObject.AddComponent<MarkerControlBehaviour>();
            _controlBehaviour.ArCameraGameObject = new GameObject();
            _controlBehaviour.ArCameraGameObject.AddComponent<Camera>();
        }

        [Test]
        public void Start_WillAddBehavioursWithCorrectEnabledState()
        {
            _controlBehaviour.Start();

            var markerFaceCameraBehaviour = _markerGameObject.GetComponent<MarkerFaceCameraBehaviour>();
            Assert.IsTrue(markerFaceCameraBehaviour.enabled);
            Assert.AreEqual(_controlBehaviour.ArCameraGameObject, markerFaceCameraBehaviour.ArCameraGameObject);
            Assert.IsFalse(_markerGameObject.GetComponent<MarkerSpinBehaviour>().enabled);
        }

        [Test]
        public void Update_WhenMarkerIsActiveWillEnableSpinBehaviourAndDisableFaceCameraBehaviour()
        {
            _controlBehaviour.Marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0) {Active = true};
            _controlBehaviour.InputHandler = new MockInputHandler(new List<Touch>());
            _controlBehaviour.PhysicsHandler = new MockPhysicsHandler<MarkerControlBehaviour>();

            _controlBehaviour.Start();

            _controlBehaviour.Update();

            var markerSpinBehaviour = _markerGameObject.GetComponent<MarkerSpinBehaviour>();

            Assert.IsTrue(markerSpinBehaviour.enabled);
            Assert.IsFalse(_markerGameObject.GetComponent<MarkerFaceCameraBehaviour>().enabled);
        }

        [Test]
        public void Update_WhenMarkerIsNotActiveThenDisableSpinBehaviourAndEnableFaceCameraBehaviour()
        {
            _controlBehaviour.Marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0);
            _controlBehaviour.InputHandler = new MockInputHandler(new List<Touch>());
            _controlBehaviour.PhysicsHandler = new MockPhysicsHandler<MarkerControlBehaviour>();
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
            _controlBehaviour.InputHandler = new MockInputHandler(new List<Touch> {touch});

            var uniqueIdentifier = "Aqua-pup Castle";
            _markerGameObject.name = uniqueIdentifier;
            _controlBehaviour.PhysicsHandler = PhysicsHandlerThatReturnsDetected();
            _controlBehaviour.Marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0);

            _controlBehaviour.Start();

            _controlBehaviour.Update();

            Assert.IsTrue(_controlBehaviour.Marker.Active);
        }

        [Test]
        public void Update_WhenTouchOnDifferentMarkerIsDetectedThisMarkerIsUnaffected()
        {
            _controlBehaviour.Marker = new Marker("Mr. Mime's Karaoke Fun Times", 0, 0);
            var touch = new Touch
                {position = new Vector2(4, 4), deltaPosition = new Vector2(2, 2), phase = TouchPhase.Began};
            _controlBehaviour.InputHandler = new MockInputHandler(new List<Touch>{touch});
            _controlBehaviour.PhysicsHandler = new MockPhysicsHandler<MarkerControlBehaviour>();
            var mockPhysicsHandler = PhysicsHandlerReturnsDifferentMarkerDetected();
            _controlBehaviour.PhysicsHandler = mockPhysicsHandler;
            _controlBehaviour.Start();

            _controlBehaviour.Update();

            Assert.IsFalse(_controlBehaviour.Marker.Active);
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