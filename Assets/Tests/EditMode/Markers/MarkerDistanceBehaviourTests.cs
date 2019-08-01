using Markers;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Tests.EditMode.Markers
{
    public class MarkerDistanceBehaviourTests
    {
        private GameObject _markerGameObject;
        private MarkerDistanceBehaviour _markerBehaviour;

        [SetUp]
        public void Setup()
        {
            _markerGameObject = new GameObject();
            _markerBehaviour = _markerGameObject.AddComponent<MarkerDistanceBehaviour>();
            _markerBehaviour.arCameraGameObject = new GameObject();
        }

        [Test]
        public void Update_GivenUserIsNotNearArMarkers_NoArMarkersAreShown()
        {
            _markerGameObject.transform.position = new Vector3(16, 0, 0);
            _markerBehaviour.arCameraGameObject.transform.position = new Vector3(0, 0, 0);

            _markerBehaviour.Update();

            Assert.IsFalse(_markerGameObject.activeSelf);
        }

        [Test]
        public void Update_GivenUserIsNearArMarkers_ArMarkersAreShown()
        {
            _markerGameObject.transform.position = new Vector3(4, 0, 0);
            _markerBehaviour.arCameraGameObject.transform.position = new Vector3(0, 0, 0);

            _markerBehaviour.Update();

            Assert.IsTrue(_markerGameObject.activeSelf);
        }

        [Test]
        public void Update_GivenUserIsNotNearArMarkers_WhenUserMovesCloser_ArMarkersAreShown()
        {
            _markerGameObject.transform.position = new Vector3(16, 0, 0);

            _markerBehaviour.arCameraGameObject.transform.position = new Vector3(0, 0, 0);
            _markerBehaviour.Update();
            Assert.IsFalse(_markerGameObject.activeSelf);

            _markerBehaviour.arCameraGameObject.transform.position = new Vector3(6, 0, 0);
            _markerBehaviour.Update();

            Assert.IsTrue(_markerGameObject.activeSelf);
        }
    }
}