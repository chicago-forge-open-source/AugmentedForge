using System.Collections.Generic;
using AR;
using NUnit.Framework;
using SyncPoints;
using Tests.PlayMode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Tests.EditMode.AR
{
    public class ArDetectImageBehaviourTests
    {
        private ArDetectImageBehaviour _script;
        private GameObject _game;

        [SetUp]
        public void SetUp()
        {
            _game = new GameObject();
            _script = _game.AddComponent<ArDetectImageBehaviour>();
            _script.calibrationBehaviour = _game.AddComponent<ArCalibrationBehaviour>();
            _script.arCamera = new GameObject();
            _script.debugText = _game.AddComponent<Text>();
            _script.detectedImageMarker = new GameObject();
        }

        [Test]
        public void
            GivenForgeSignImageIsDetectedWithNoDistanceFromCamera_LocationIsSetToForgeSignSyncPointWith180Orientation()
        {
            const string name = "Rob's Place";
            var definedSyncPoint = new SyncPoint(name, 10f, 15f, 180);
            Repositories.SyncPointRepository.Save(new[] {definedSyncPoint});
            _script.compass = new MockCompass {TrueHeading = 180f};

            var forgeSignImg = _game.AddComponent<ARTrackedImage>();
            _script.getTrackingState = image => TrackingState.Tracking;
            _script.getReferenceName = image => name;
            forgeSignImg.transform.position = new Vector3(definedSyncPoint.X, 2.5f, definedSyncPoint.Z);
            forgeSignImg.transform.rotation = Quaternion.Euler(0, definedSyncPoint.Orientation, 0);

            _script.arCamera.transform.position = new Vector3(definedSyncPoint.X, 2.5f, definedSyncPoint.Z);
            _script.arCamera.transform.rotation = Quaternion.Euler(0, definedSyncPoint.Orientation, 0);


            var events = new ARTrackedImagesChangedEventArgs(
                new List<ARTrackedImage>(),
                new List<ARTrackedImage> {forgeSignImg},
                new List<ARTrackedImage>()
            );

            _script.OnTrackedImagesChanged(events);
            var expectedSyncPoint = new SyncPoint(definedSyncPoint.Name, definedSyncPoint.X, definedSyncPoint.Z,
                180f);
            Assert.AreEqual(expectedSyncPoint, _script.calibrationBehaviour.pendingSyncPoint);
        }

        [Test]
        public void
            GivenForgeSignImageIsDetectedWithDistanceFromCamera_LocationIsOffSetByDistanceFromSign_WithinRanges()
        {
            const string name = "Test Two";
            var knownSyncPoint = new SyncPoint(name, 1000f, 100f, 90);
            Repositories.SyncPointRepository.Save(new[] {knownSyncPoint});
            _script.compass = new MockCompass {TrueHeading = 266f};

            var forgeSignImg = _game.AddComponent<ARTrackedImage>();
            _script.getTrackingState = image => TrackingState.Tracking;
            forgeSignImg.transform.position = new Vector3(1002f, 2.5f, 102f);
            _script.getReferenceName = image => name;
            forgeSignImg.transform.rotation = Quaternion.Euler(0, 258.34f, 0);

            var events = new ARTrackedImagesChangedEventArgs(
                new List<ARTrackedImage>(),
                new List<ARTrackedImage> {forgeSignImg},
                new List<ARTrackedImage>()
            );

            _script.arCamera.transform.position = new Vector3(900, 2.5f, 80);
            _script.arCamera.transform.rotation = Quaternion.Euler(0, 254.34f, 0);

            _script.OnTrackedImagesChanged(events);
            var expectedSyncPoint = new SyncPoint(name, 898, 78f, 266f);
            Assert.AreEqual(expectedSyncPoint, _script.calibrationBehaviour.pendingSyncPoint);
        }

        [Test]
        public void GivenForgeSignImageIsDetectedWithDistanceFromCamera_LocationIsNotSetWhenOutsideOrientationRange()
        {
            const string name = "Test Tres";
            var knownSyncPoint = new SyncPoint(name, 1000f, 100f, 90);
            Repositories.SyncPointRepository.Save(new[] {knownSyncPoint});

            var forgeSignImg = _game.AddComponent<ARTrackedImage>();
            _script.getTrackingState = image => TrackingState.Tracking;
            forgeSignImg.transform.position = new Vector3(1002f, 2.5f, 102f);
            _script.getReferenceName = image => name;
            forgeSignImg.transform.rotation = Quaternion.Euler(0, 258.34f, 0);

            var events = new ARTrackedImagesChangedEventArgs(
                new List<ARTrackedImage>(),
                new List<ARTrackedImage> {forgeSignImg},
                new List<ARTrackedImage>()
            );

            _script.arCamera.transform.position = new Vector3(900, 2.5f, 80);
            _script.arCamera.transform.rotation = Quaternion.Euler(0, 240.34f, 0);

            _script.OnTrackedImagesChanged(events);

            Assert.AreEqual(null, _script.calibrationBehaviour.pendingSyncPoint);
        }
    }
}