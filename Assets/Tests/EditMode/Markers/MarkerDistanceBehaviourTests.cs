using Markers;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Markers
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
            SetMarkerObjectPosition(16);
            SetCameraPosition();

            _markerBehaviour.Update();

            Assert.IsFalse(_markerGameObject.activeSelf);
        }

        [Test]
        public void Update_GivenUserIsNearArMarkers_ArMarkersAreShown()
        {
            SetMarkerObjectPosition(4);
            SetCameraPosition();

            _markerBehaviour.Update();

            Assert.IsTrue(_markerGameObject.activeSelf);
        }

        [Test]
        public void Update_GivenUserIsNotNearArMarkers_WhenUserMovesCloser_ArMarkersAreShown()
        {
            SetMarkerObjectPosition(16);

            SetCameraPosition();
            _markerBehaviour.Update();
            Assert.IsFalse(_markerGameObject.activeSelf);

            SetCameraPosition(6);
            _markerBehaviour.Update();

            Assert.IsTrue(_markerGameObject.activeSelf);
        }

        private void SetMarkerObjectPosition(float x = 0, float z = 0)
        {
            _markerGameObject.transform.position = new Vector3(x, 0, z);
        }

        private void SetCameraPosition(float x = 0, float z = 0)
        {
            _markerBehaviour.arCameraGameObject.transform.position = new Vector3(x, 0, z);
        }
    }
}