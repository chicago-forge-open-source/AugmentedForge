using Assets.Scripts.Markers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Assets.Tests.EditMode.Markers
{
    public class MarkerFaceCameraBehaviourTests
    {
        private GameObject _markerGameObject;
        private MarkerFaceCameraBehaviour _markerBehaviour;
        private readonly QuaternionEqualityComparer _quaternionComparer = new QuaternionEqualityComparer(10e-6f);

        [SetUp]
        public void Setup()
        {
            _markerGameObject = new GameObject();
            _markerBehaviour = _markerGameObject.AddComponent<MarkerFaceCameraBehaviour>();
            _markerBehaviour.arCameraGameObject = new GameObject();
        }

        [Test]
        public void Update_GivenTheModelFacesEastNaturally_RotateNorthMarkerToFaceArCameraLocation()
        {
            _markerGameObject.transform.position = new Vector3(1, 0, 0);
            _markerBehaviour.arCameraGameObject.transform.position = new Vector3(0, 0, 0);

            _markerBehaviour.Update();

            Assert.That(_markerGameObject.transform.rotation,
                Is.EqualTo(Quaternion.Euler(0, 270, 0)).Using(_quaternionComparer)
            );
        }
        
        [Test]
        public void Update_GivenTheModelFacesEastNaturally_RotateWestMarkerToFaceArCameraLocation()
        {
            _markerGameObject.transform.position = new Vector3(0, 0, 1);
            _markerBehaviour.arCameraGameObject.transform.position = new Vector3(0, 0, 0);

            _markerBehaviour.Update();

            Assert.That(_markerGameObject.transform.rotation,
                Is.EqualTo(Quaternion.Euler(0, 180, 0)).Using(_quaternionComparer)
            );
        }


        [Test]
        public void Update_ArMarkersDoNotChangeRotationOnXOrZAxises()
        {
            _markerGameObject.transform.position = new Vector3(1, 0, 0);
            _markerBehaviour.arCameraGameObject.transform.position = new Vector3(0, 100, 0);

            _markerBehaviour.Update();

            Assert.That(_markerGameObject.transform.rotation,
                Is.EqualTo(Quaternion.Euler(0, 270, 0)).Using(_quaternionComparer)
            );
        }
    }
}