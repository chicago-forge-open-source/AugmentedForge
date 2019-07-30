using Assets.Scripts.Markers;
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
            _markerBehaviour.ArCameraGameObject = new GameObject();
        }

        [Test]
        public void Update_GivenUserIsNotNearArMarkers_NoArMarkersAreShown()
        {
            _markerGameObject.transform.position = new Vector3(11, 0, 0);
            _markerBehaviour.ArCameraGameObject.transform.position = new Vector3(0, 0, 0);

            _markerBehaviour.Update();

            Assert.False(_markerGameObject.activeSelf);
        }

        [Test]
        public void Update_GivenUserIsNearArMarkers_ArMarkersAreShown()
        {
            _markerGameObject.transform.position = new Vector3(4, 0, 0);
            _markerBehaviour.ArCameraGameObject.transform.position = new Vector3(0, 0, 0);

            _markerBehaviour.Update();

            Assert.True(_markerGameObject.activeSelf);
        }

        [Test]
        public void Update_GivenUserIsNotNearArMarkers_WhenUserMovesCloser_ArMarkersAreShown()
        {
            _markerGameObject.transform.position = new Vector3(10, 0, 0);

            _markerBehaviour.ArCameraGameObject.transform.position = new Vector3(0, 0, 0);
            _markerBehaviour.Update();
            _markerBehaviour.ArCameraGameObject.transform.position = new Vector3(6, 0, 0);

            _markerBehaviour.Update();

            Assert.True(_markerGameObject.activeSelf);
        }
    }
}