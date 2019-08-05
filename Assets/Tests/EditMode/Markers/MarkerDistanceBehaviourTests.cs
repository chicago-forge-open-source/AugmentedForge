using Markers;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Markers
{
    public class MarkerDistanceBehaviourTests
    {
        private GameObject _markerGameObject;
        private MarkerDistanceBehaviour _markerBehaviour;
        private Vector3 hiddenScale = new Vector3(0, 0, 0);
        private Vector3 showingScale = new Vector3(1, 1, 1);

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

            Assert.AreEqual(hiddenScale, _markerGameObject.transform.localScale);
        }

        [Test]
        public void Update_GivenUserIsNearArMarkers_ArMarkersAreShown()
        {
            SetMarkerObjectPosition(4);
            SetCameraPosition();

            _markerBehaviour.Update();

            Assert.AreEqual(showingScale, _markerGameObject.transform.localScale);
        }

        [Test]
        public void Update_GivenUserIsNotNearArMarkers_WhenUserMovesCloser_ArMarkersAreShown()
        {
            SetMarkerObjectPosition();

            SetCameraPosition(16);
            _markerBehaviour.Update();
            Assert.AreEqual(hiddenScale, _markerGameObject.transform.localScale);

            SetCameraPosition();
            _markerBehaviour.Update();

            Assert.AreEqual(showingScale, _markerGameObject.transform.localScale);
        }

        [Test]
        public void Update_GivenUserIsNearArMarkers_WhenUserMovesAway_ArMarkersAreNotShown()
        {
            SetMarkerObjectPosition();

            SetCameraPosition();
            _markerBehaviour.Update();
            Assert.AreEqual(showingScale, _markerGameObject.transform.localScale);

            SetCameraPosition(16);
            _markerBehaviour.Update();

            Assert.AreEqual(hiddenScale, _markerGameObject.transform.localScale);
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