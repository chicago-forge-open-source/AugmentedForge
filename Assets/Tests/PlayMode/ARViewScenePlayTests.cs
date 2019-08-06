using System;
using System.Collections;
using Locations;
using Markers;
using NUnit.Framework;
using Roads;
using SyncPoints;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Tests.PlayMode
{
    public class ArViewPlayTests
    {
        private GameObject _mainCamera;

        private IEnumerator LoadScene()
        {
            Repositories.LocationsRepository.Save(new []{new Location("", "ChicagoMap") });
            SceneManager.LoadScene("ARView");
            yield return null;
            _mainCamera = GameObject.Find("Map Camera");
            
        }

        [UnityTest]
        [UnityPlatform(RuntimePlatform.Android, RuntimePlatform.IPhonePlayer)]
        public IEnumerator WhenCompassEnabledCameraIsRotated()
        {
            yield return LoadScene();
            var comparer = new QuaternionEqualityComparer(10e-6f);
            var defaultQuaternion = Quaternion.Euler(0, 0, 0);
            yield return new WaitUntil(() => Math.Abs(Input.compass.trueHeading) > 0);
            Assert.That(_mainCamera.transform.rotation, Is.Not.EqualTo(defaultQuaternion).Using(comparer));
        }

        [UnityTest]
        public IEnumerator OnStartLocationMarkerIsSetToSyncPoint()
        {
            var syncPoint = new SyncPoint(10, 10);
            Repositories.SyncPointRepository.Save(new []{syncPoint});
            
            yield return LoadScene();
            var locationMarker = GameObject.Find("Location Marker");

            var locationMarkerPosition = locationMarker.transform.position;
            Assert.AreEqual(syncPoint.X, locationMarkerPosition.x);
            Assert.AreEqual(syncPoint.Z, locationMarkerPosition.z);
            yield return null;
        }

        [UnityTest]
        public IEnumerator OnStartMapSpriteWillLoadIntoOverlayMap()
        {
            
            const string location = "Chicago";
            PlayerPrefs.SetString("location", location);
            Repositories.SyncPointRepository.Save(new []{new SyncPoint(10, 10), });
            
            yield return LoadScene();
            var map = GameObject.Find("Overlay Map");
            var mapScript = map.GetComponent<OverlayMapBehaviour>();

            mapScript.Start();

            yield return null;
            var mapSprite = map.GetComponent<SpriteRenderer>().sprite;

            Assert.AreEqual(location + "MapSprite", mapSprite.name);
        }

        [UnityTest]
        public IEnumerator OnStartCameraViewsLocationMarker()
        {
            var syncPoint = new SyncPoint(10, 10);
            Repositories.SyncPointRepository.Save(new []{syncPoint});
            yield return LoadScene();

            var cameraPos = _mainCamera.transform.position;
            yield return null;
            Assert.AreEqual(syncPoint.X, cameraPos.x);
            Assert.AreEqual(syncPoint.Z, cameraPos.z);
        }

        [UnityTest]
        [UnityPlatform(RuntimePlatform.Android, RuntimePlatform.IPhonePlayer)]
        public IEnumerator OnRealWorldChangeInLocationLocationMarkerChangesPosition()
        {
            yield return LoadScene();
            var locationMarker = GameObject.Find("Location Marker");
            var initialPosition = locationMarker.transform.position;

            yield return new WaitForSeconds(5);

            // ReSharper disable once Unity.InefficientPropertyAccess
            Assert.AreNotEqual(initialPosition, locationMarker.transform.position);
        }

        [TearDown]
        public void Dispose()
        {
            Repositories.LocationsRepository.Save(new Location[]{});
            Repositories.MarkerRepository.Save(new Marker[]{});
            Repositories.RoadRepository.Save(new Road[]{});
            Repositories.SyncPointRepository.Save(new SyncPoint[]{});
        }
    }
}