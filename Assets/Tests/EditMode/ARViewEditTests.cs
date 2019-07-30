using Assets.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Assets.Tests.EditMode
{
    public class ArViewEditTests
    {
        private GameObject _game;
        private readonly QuaternionEqualityComparer _quaternionComparer = new QuaternionEqualityComparer(10e-6f);
        private ARView _mapScript;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _game.AddComponent<SpriteRenderer>();
            _mapScript = _game.AddComponent<ARView>();
            _mapScript.DebugText = _game.AddComponent<Text>();

            _mapScript.ArCameraComponent = new GameObject();
            _mapScript.ArCameraComponent.AddComponent<ARCameraBackground>();
            _mapScript.ArCameraComponent.GetComponent<Camera>().cullingMask = 567;

            _mapScript.StartPoint = new GameObject();
            _mapScript.ArSessionOrigin = new GameObject();
        }

        [Test]
        public void Start_WillMoveArOriginToStartPoint()
        {
            var startPosition = new Vector3(3, 6, 9);
            _mapScript.StartPoint.transform.position = startPosition;

            _mapScript.Start();

            var position = _mapScript.ArSessionOrigin.transform.position;
            var expectedVector = new Vector3(startPosition.x, 0, startPosition.z);
            Assert.AreEqual(expectedVector, position);
        }

        [Test]
        public void Start_WillUpdateArSessionOriginRotationBasedOnCompass()
        {
            _mapScript.Compass = new MockCompass {TrueHeading = 180f};

            _mapScript.Start();

            var expectedCameraRotation = Quaternion.Euler(0, 180f, 0);

            Assert.That(_mapScript.ArSessionOrigin.transform.rotation,
                Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer)
            );
        }
    }
}