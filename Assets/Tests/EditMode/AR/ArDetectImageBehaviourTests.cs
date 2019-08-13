using System.Collections.Generic;
using AR;
using NUnit.Framework;
using SyncPoints;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

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
        }

        [Test]
        public void GivenForgeSignImageIsDetected_LocationIsSetToForgeSignSyncPoint()
        {
            const string name = "Rob's Place";
            var expectedSyncPoint = new SyncPoint(name, 10f, 15f, 180);
            Repositories.SyncPointRepository.Save(new[] {expectedSyncPoint});

            var forgeSignImg = _game.AddComponent<ARTrackedImage>();
            forgeSignImg.name = name;

            var events = new ARTrackedImagesChangedEventArgs(
                new List<ARTrackedImage>(),
                new List<ARTrackedImage> {forgeSignImg},
                new List<ARTrackedImage>()
            );

            // add transformation position to image, update expectation to include distance from camera
            _script.OnTrackedImagesChanged(events);

            Assert.AreEqual(expectedSyncPoint, _script.calibrationBehaviour.pendingSyncPoint);
        }
    }
}