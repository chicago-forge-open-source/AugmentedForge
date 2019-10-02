using System.Linq;
using Markers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Tests.EditMode
{
    public class HudBehaviourTests
    {
        private GameObject _game;
        private HudBehaviour _mapScript;
        private const string Chicago = "Chicago";

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _game.AddComponent<SpriteRenderer>();
            _mapScript = _game.AddComponent<HudBehaviour>();

            _mapScript.mapCameraGameObject = new GameObject();
            _mapScript.mapCameraGameObject.AddComponent<Camera>();
            _mapScript.mapCameraGameObject.AddComponent<FingerGestures>();

            _mapScript.arCameraGameObject = new GameObject();
            _mapScript.arCameraGameObject.AddComponent<ARCameraBackground>();
            _mapScript.arCameraGameObject.GetComponent<Camera>().cullingMask = 567;

            _mapScript.arMapOverlayToggle = new GameObject();
            _mapScript.arMapOverlayToggle.AddComponent<Button>();

            _mapScript.backButton = new GameObject();
            _mapScript.backButton.AddComponent<Button>();

            _mapScript.presentMarkersBehaviour = _game.AddComponent<PresentMarkersBehaviour>();

            PlayerPrefs.SetString("location", Chicago);
        }

        [Test]
        public void GivenButtonToggleAndMapViewInArShowingHideTheMap()
        {
            _mapScript.Start();
            var camera = _mapScript.mapCameraGameObject.GetComponent<Camera>();
            camera.enabled = true;

            _mapScript.OnClick_ArMapOverlayToggle();

            Assert.IsFalse(camera.enabled);
        }

        [Test]
        public void GivenButtonToggleAndMapViewInArHidingShowTheMap()
        {
            _mapScript.Start();
            var camera = _mapScript.mapCameraGameObject.GetComponent<Camera>();
            camera.enabled = false;

            _mapScript.OnClick_ArMapOverlayToggle();

            Assert.IsTrue(camera.enabled);
        }

        [Test]
        public void MapCameraIsAlwaysShown()
        {
            _mapScript.Start();
            var camera = _mapScript.mapCameraGameObject.GetComponent<Camera>();
            camera.enabled = false;

            _mapScript.OnClick_MapOnlyToggle();

            Assert.IsTrue(camera.enabled);
        }

        [Test]
        public void GivenARBackgroundIsEnabled_ARBackgroundIsHidden()
        {
            _mapScript.Start();
            var background = _mapScript.arCameraGameObject.GetComponent<ARCameraBackground>();

            _mapScript.OnClick_MapOnlyToggle();

            Assert.IsFalse(background.enabled);
        }

        [Test]
        public void GivenARBackgroundIsHidden_ARBackgroundIsShown()
        {
            _mapScript.Start();
            var background = _mapScript.arCameraGameObject.GetComponent<ARCameraBackground>();
            background.enabled = false;

            _mapScript.OnClick_MapOnlyToggle();

            Assert.IsTrue(background.enabled);
        }

        [Test]
        public void GivenArBackgroundTogglesToDisabled_FingerGesturesEnabled()
        {
            _mapScript.Start();
            var gesturesScript = _mapScript.mapCameraGameObject.GetComponent<FingerGestures>();
            gesturesScript.enabled = false;

            _mapScript.OnClick_MapOnlyToggle();

            Assert.IsTrue(gesturesScript.enabled);
        }

        [Test]
        public void GivenArBackgroundTogglesToEnabled_FingerGesturesDisabled()
        {
            StartInMapOnlyMode();

            _mapScript.OnClick_MapOnlyToggle();

            Assert.IsFalse(_mapScript.mapCameraGameObject.GetComponent<FingerGestures>().enabled);
        }

        [Test]
        public void GivenArBackgroundTogglesToEnabled_MapCameraZoomIsSetToInitialValue()
        {
            StartInMapOnlyMode();
            var camera = _mapScript.mapCameraGameObject.GetComponent<Camera>();
            camera.fieldOfView = 80f;

            _mapScript.OnClick_MapOnlyToggle();

            Assert.AreEqual(60f, camera.fieldOfView);
        }

        [Test]
        public void GivenArBackgroundTogglesToDisabled_MapCameraRotationIsSetToInitialValue()
        {
            _mapScript.Start();
            var camera = _mapScript.mapCameraGameObject.GetComponent<Camera>();
            camera.transform.rotation = Quaternion.Euler(100, 10, 70);

            _mapScript.OnClick_MapOnlyToggle();

            TestHelpers.AssertQuaternionsAreEqual(
                Quaternion.Euler(90, 0, 0),
                _mapScript.mapCameraGameObject.transform.rotation
            );
        }

        [Test]
        public void GivenArBackgroundTogglesToDisabled_ArCameraMaskDoesNotIncludeLayerNine()
        {
            _mapScript.Start();

            _mapScript.OnClick_MapOnlyToggle();

            var actualMask = _mapScript.arCameraGameObject.GetComponent<Camera>().cullingMask;
            Assert.AreEqual(0, actualMask & (1 << 9));
        }

        [Test]
        public void GivenArBackgroundTogglesToEnabled_ArCameraMaskIncludesLayerNine()
        {
            StartInMapOnlyMode();
            var arCamera = _mapScript.arCameraGameObject.GetComponent<Camera>();
            arCamera.cullingMask &= ~(1 << 9);

            _mapScript.OnClick_MapOnlyToggle();

            var actualMask = _mapScript.arCameraGameObject.GetComponent<Camera>().cullingMask;
            Assert.AreNotEqual(0, actualMask & (1 << 9));
        }

        private void StartInMapOnlyMode()
        {
            _mapScript.Start();
            _mapScript.arCameraGameObject.GetComponent<ARCameraBackground>().enabled = false;
        }

        [Test]
        public void GivenShowingMapOnly_ArMapOverlayToggleIsDisabled()
        {
            _mapScript.Start();

            _mapScript.OnClick_MapOnlyToggle();
            Assert.IsFalse(_mapScript.arMapOverlayToggle.activeSelf);
        }

        [Test]
        public void GivenShowingArView_ArMapOverlayToggleIsEnabled()
        {
            _mapScript.Start();

            _mapScript.OnClick_MapOnlyToggle();
            _mapScript.OnClick_MapOnlyToggle();
            Assert.IsTrue(_mapScript.arMapOverlayToggle.activeSelf);
        }
        
        [Test]
        public void GivenShowingMapOnly_BackButtonIsDisabled()
        {
            _mapScript.Start();

            _mapScript.OnClick_MapOnlyToggle();
            Assert.IsFalse(_mapScript.backButton.activeSelf);
        }

        [Test]
        public void GivenShowingArView_BackButtonIsEnabled()
        {
            _mapScript.Start();

            _mapScript.OnClick_MapOnlyToggle();
            _mapScript.OnClick_MapOnlyToggle();
            Assert.IsTrue(_mapScript.backButton.activeSelf);
        }

        [Test]
        public void OnClick_GivenMapOnlyMarkersAreReadable()
        {
            _mapScript.Start();

            GameObject north = new GameObject("north");
            north.transform.rotation = Quaternion.Euler(1, 1, 1);
            _mapScript.presentMarkersBehaviour.MapMarkers.Add(north);

            _mapScript.OnClick_MapOnlyToggle();

            var markerRotation = _mapScript.presentMarkersBehaviour.MapMarkers
                .First(marker => marker.name.Equals("north")).transform.rotation;

            TestHelpers.AssertQuaternionsAreEqual(
                Quaternion.Euler(90, 0, 0),
                markerRotation
            );
        }
    }
}