using System.Linq;
using Assets.Scripts.Marker;
using AugmentedForge;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Assets.Tests.EditMode
{
    public class ControllerBehaviourEditTests
    {
        private GameObject _game;
        private readonly QuaternionEqualityComparer _quaternionComparer = new QuaternionEqualityComparer(10e-6f);
        private ControllerBehaviour _mapScript;
        private const string Chicago = "Chicago";

        [SetUp]
        public void Setup()
        {
            _game = new GameObject();
            _game.AddComponent<SpriteRenderer>();
            _mapScript = _game.AddComponent<ControllerBehaviour>();

            _mapScript.MapCameraComponent = new GameObject();
            _mapScript.MapCameraComponent.AddComponent<Camera>();
            _mapScript.MapCameraComponent.AddComponent<FingerGestures>();

            _mapScript.ArCameraComponent = new GameObject();
            _mapScript.ArCameraComponent.AddComponent<ARCameraBackground>();
            _mapScript.ArCameraComponent.GetComponent<Camera>().cullingMask = 567;

            _mapScript.ArMapOverlayToggle = new GameObject();
            _mapScript.ArMapOverlayToggle.AddComponent<Button>();
            
            _mapScript.MarkerBehaviour = _game.AddComponent<MarkerBehaviour>();

            PlayerPrefs.SetString("location", Chicago);
        }

        [Test]
        public void GivenButtonToggleAndMapViewInArShowingHideTheMap()
        {
            _mapScript.Start();
            var camera = _mapScript.MapCameraComponent.GetComponent<Camera>();
            camera.enabled = true;

            _mapScript.OnClick_ArMapOverlayToggle();

            Assert.IsFalse(camera.enabled);
        }

        [Test]
        public void GivenButtonToggleAndMapViewInArHidingShowTheMap()
        {
            _mapScript.Start();
            var camera = _mapScript.MapCameraComponent.GetComponent<Camera>();
            camera.enabled = false;

            _mapScript.OnClick_ArMapOverlayToggle();

            Assert.IsTrue(camera.enabled);
        }

        [Test]
        public void MapCameraIsAlwaysShown()
        {
            _mapScript.Start();
            var camera = _mapScript.MapCameraComponent.GetComponent<Camera>();
            camera.enabled = false;

            _mapScript.OnClick_MapOnlyToggle();

            Assert.IsTrue(camera.enabled);
        }

        [Test]
        public void GivenARBackgroundIsEnabled_ARBackgroundIsHidden()
        {
            _mapScript.Start();
            var background = _mapScript.ArCameraComponent.GetComponent<ARCameraBackground>();

            _mapScript.OnClick_MapOnlyToggle();

            Assert.IsFalse(background.enabled);
        }

        [Test]
        public void GivenARBackgroundIsHidden_ARBackgroundIsShown()
        {
            _mapScript.Start();
            var background = _mapScript.ArCameraComponent.GetComponent<ARCameraBackground>();
            background.enabled = false;

            _mapScript.OnClick_MapOnlyToggle();

            Assert.IsTrue(background.enabled);
        }

        [Test]
        public void GivenArBackgroundTogglesToDisabled_FingerGesturesEnabled()
        {
            _mapScript.Start();
            var gesturesScript = _mapScript.MapCameraComponent.GetComponent<FingerGestures>();
            gesturesScript.enabled = false;

            _mapScript.OnClick_MapOnlyToggle();

            Assert.IsTrue(gesturesScript.enabled);
        }

        [Test]
        public void GivenArBackgroundTogglesToEnabled_FingerGesturesDisabled()
        {
            StartInMapOnlyMode();

            _mapScript.OnClick_MapOnlyToggle();

            Assert.IsFalse(_mapScript.MapCameraComponent.GetComponent<FingerGestures>().enabled);
        }

        [Test]
        public void GivenArBackgroundTogglesToEnabled_MapCameraZoomIsSetToInitialValue()
        {
            StartInMapOnlyMode();
            var camera = _mapScript.MapCameraComponent.GetComponent<Camera>();
            camera.fieldOfView = 80f;

            _mapScript.OnClick_MapOnlyToggle();

            Assert.AreEqual(60f, camera.fieldOfView);
        }

        [Test]
        public void GivenArBackgroundTogglesToDisabled_MapCameraRotationIsSetToInitialValue()
        {
            _mapScript.Start();
            var camera = _mapScript.MapCameraComponent.GetComponent<Camera>();
            camera.transform.rotation = Quaternion.Euler(100, 10, 70);

            _mapScript.OnClick_MapOnlyToggle();

            var expectedCameraRotation = Quaternion.Euler(90, 0, 0);
            Assert.That(_mapScript.MapCameraComponent.transform.rotation,
                Is.EqualTo(expectedCameraRotation).Using(_quaternionComparer)
            );
        }

        [Test]
        public void GivenArBackgroundTogglesToDisabled_ArCameraMaskDoesNotIncludeLayerNine()
        {
            _mapScript.Start();

            _mapScript.OnClick_MapOnlyToggle();

            var actualMask = _mapScript.ArCameraComponent.GetComponent<Camera>().cullingMask;
            Assert.AreEqual(0, actualMask & (1 << 9));
        }

        [Test]
        public void GivenArBackgroundTogglesToEnabled_ArCameraMaskIncludesLayerNine()
        {
            StartInMapOnlyMode();
            var arCamera = _mapScript.ArCameraComponent.GetComponent<Camera>();
            arCamera.cullingMask &= ~(1 << 9);

            _mapScript.OnClick_MapOnlyToggle();

            var actualMask = _mapScript.ArCameraComponent.GetComponent<Camera>().cullingMask;
            Assert.AreNotEqual(0, actualMask & (1 << 9));
        }

        private void StartInMapOnlyMode()
        {
            _mapScript.Start();
            _mapScript.ArCameraComponent.GetComponent<ARCameraBackground>().enabled = false;
        }
    
        [Test]
        public void GivenShowingMapOnly_ArMapOverlayToggleIsDisabled()
        {
            _mapScript.Start();

            _mapScript.OnClick_MapOnlyToggle();
            Assert.IsFalse(_mapScript.ArMapOverlayToggle.activeSelf);
        }
    
        [Test]
        public void GivenShowingArView_ArMapOverlayToggleIsEnabled()
        {
            _mapScript.Start();

            _mapScript.OnClick_MapOnlyToggle();
            _mapScript.OnClick_MapOnlyToggle();
            Assert.True(_mapScript.ArMapOverlayToggle.activeSelf);
        }

        [Test]
        public void OnClick_GivenMapOnlyMarkersAreReadable()
        {
            _mapScript.Start();
            
            GameObject north = new GameObject("north");
            north.transform.rotation = Quaternion.Euler(1,1,1);
            _mapScript.MarkerBehaviour.MapMarkers.Add(north);
            
            _mapScript.OnClick_MapOnlyToggle();
            
            Assert.AreEqual(Quaternion.Euler(90,0,0).eulerAngles,_mapScript.MarkerBehaviour.MapMarkers.First(marker => marker.name.Equals("north")).transform.rotation.eulerAngles);
        }
    }

}