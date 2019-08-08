using System;
using DataLoaders;
using Markers;
using DefaultNamespace;
using NUnit.Framework;
using SyncPoints;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Tests.EditMode
{
    public class ArViewEditTests
    {
        private GameObject _game;
        private ARView _mapScript;

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _game.AddComponent<SpriteRenderer>();
            _mapScript = _game.AddComponent<ARView>();
            _mapScript.debugText = _game.AddComponent<Text>();

            _mapScript.arCameraGameObject = new GameObject();
            _mapScript.arCameraGameObject.AddComponent<ARCameraBackground>();
            _mapScript.arCameraGameObject.GetComponent<Camera>().cullingMask = 567;

            _mapScript.startPoint = new GameObject();
            _mapScript.arSessionOrigin = new GameObject();
            _mapScript.arSessionOrigin.AddComponent<ARSessionOrigin>();

            _mapScript.scrollContent = new GameObject();
            SetupScrollItemPrefab();
        }

        [Test]
        public void Start_WillMoveArOriginToStartPoint()
        {
            _mapScript.Start();

            var position = _mapScript.arSessionOrigin.transform.position;
            Assert.AreEqual(_mapScript.startPoint.transform.position, position);
        }

        [Test]
        public void Start_WillUpdateArSessionOriginRotationBasedOnCompass()
        {
            _mapScript.compass = new MockCompass {TrueHeading = 180f};

            _mapScript.Start();

            TestHelpers.AssertQuaternionsAreEqual(
                Quaternion.Euler(0, 180f, 0),
                _mapScript.arSessionOrigin.transform.rotation
            );
        }


        [Test]
        public void Start_GivenALocation_ExpectedSyncPointIsLoaded()
        {
            new ChicagoDataLoader().DataLoad();
            var expectedSyncPointPosition = new Vector3(0, 0, -1.5f);
            PlayerSelections.startingParametersProvided = false;
            Repositories.SyncPointRepository.Save(new[]
                {new SyncPoint(expectedSyncPointPosition.x, expectedSyncPointPosition.z)});
            _mapScript.Start();

            var actualSyncPointPosition = _mapScript.startPoint.transform.position;
            //TODO: Move to Vector3 Comparer (may want to add to TestHelpers class now and update other references)
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.x - actualSyncPointPosition.x) < .1);
            Assert.IsTrue(Math.Abs(expectedSyncPointPosition.z - actualSyncPointPosition.z) < .01);
        }
        
        [Test]
        public void GivenAPlayerStartPointIsProvided_SyncPointIsSetToPlayerStartParameters()
        {
            new ChicagoDataLoader().DataLoad();
            var expectedPosition = new Vector3(1, 0, 1);
            PlayerSelections.startingPoint = expectedPosition;
            var expectedYRotation = 90f;
            PlayerSelections.directionInYRotation = expectedYRotation;
            PlayerSelections.startingParametersProvided = true;

            _mapScript.Start();

            var actualPosition = _mapScript.startPoint.transform.position;
            Assert.AreEqual(expectedPosition.x, actualPosition.x);
            Assert.AreEqual(expectedPosition.z, actualPosition.z);
            TestHelpers.AssertQuaternionsAreEqual(
                Quaternion.Euler(0, expectedYRotation, 0),
                _mapScript.arSessionOrigin.transform.rotation);
        }

        [Test]
        public void Start_WillCreateScrollViewItemOnScrollView()
        {
            var marker1 = new Marker("Lone Cowboy", 10, 10);
            Repositories.MarkerRepository.Save(new[] {marker1});

            _mapScript.Start();

            var content = _mapScript.scrollContent;

            Assert.AreEqual(1, content.transform.childCount);
        }

        [Test]
        public void Start_WillCreateScrollViewItemFromMarkers()
        {
            var marker1 = new Marker("Testing 1", 10, 10);
            var marker2 = new Marker("Testing 2", 5, 5);
            Repositories.MarkerRepository.Save(new[] {marker1, marker2});

            _mapScript.Start();

            var content = _mapScript.scrollContent;

            Assert.AreEqual(2, content.transform.childCount);
            Assert.AreEqual("ScrollItem-" + marker1.Label, content.transform.GetChild(0).name);
            Assert.AreEqual(marker1.Label, GetTextFromScrollItem(content, 0));
            Assert.AreEqual("ScrollItem-" + marker2.Label, content.transform.GetChild(1).name);
            Assert.AreEqual(marker2.Label, GetTextFromScrollItem(content, 1));
        }

        private static string GetTextFromScrollItem(GameObject content, int index)
        {
            return content.transform.GetChild(index).GetComponentInChildren<Text>().text;
        }

        private void SetupScrollItemPrefab()
        {
            _mapScript.scrollItemPrefab = new GameObject("Scroll Item Generic");
            _mapScript.scrollItemPrefab.AddComponent<Button>();

            var textChild = new GameObject();
            textChild.transform.parent = _mapScript.scrollItemPrefab.transform;
            textChild.AddComponent<Text>().text = "Blank Scroll Item";
        }
    }
}