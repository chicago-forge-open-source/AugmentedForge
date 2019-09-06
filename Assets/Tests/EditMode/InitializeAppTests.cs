using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLoaders;
using DefaultNamespace;
using Editor;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class InitializeAppTests
    {
//        [Test]
//        public void DeleteMe()
//        {
//            Export.doIoTThing()
//                .Wait();
//            
//            
//            Assert.IsTrue(true);
//        }
        
        [Test]
        public void OnLocationButtonClick_LoadsCorrectDataForThatLocation()
        {
            var initScript = new GameObject().AddComponent<InitializeApp>();

            initScript.OnClick_LoadLocationARView("Chicago");

            Assert.AreEqual(typeof(ChicagoDataLoader), initScript.dataLoader.GetType());
            Assert.IsTrue(Repositories.MarkerRepository.Get().Length > 0);
            Assert.IsTrue(Repositories.RoadRepository.Get().Length > 0);
            Assert.IsTrue(Repositories.SyncPointRepository.Get().Length > 0);
        }
        
        [Test]
        public void OnLocationButtonClick_LoadsCorrectDataForAnotherLocation()
        {
            var initScript = new GameObject().AddComponent<InitializeApp>();

            initScript.OnClick_LoadLocationARView("Iowa");

            Assert.AreEqual(typeof(IowaDataLoader), initScript.dataLoader.GetType());
            Assert.IsTrue(Repositories.MarkerRepository.Get().Length > 0);
            Assert.IsTrue(Repositories.RoadRepository.Get().Length > 0);
            Assert.IsTrue(Repositories.SyncPointRepository.Get().Length > 0);
        }
        
        [Test]
        public void GivenStartPointProvided_UserStartPointIsSetAndFacesSouth()
        {
            var initScript = new GameObject().AddComponent<InitializeApp>();
            
            Dictionary<String, object> parameters = new Dictionary<string, object>();
            parameters.Add("z", 1);
            parameters.Add("x", 1);
            parameters.Add("direction", 180);
            
            initScript.BranchCallbackWithParams(parameters,null);

            var actualLocation = PlayerSelections.qrPoint;
            Assert.AreEqual(1, actualLocation.x);
            Assert.AreEqual(1, actualLocation.z);
            Assert.AreEqual(180, PlayerSelections.orientation);
            Assert.True(PlayerSelections.qrParametersProvided);
        }
        
        [Test]
        public void GivenNoStartPointProvided_UserStartPointIsNotSet()
        {
            var initScript = new GameObject().AddComponent<InitializeApp>();
            
            Dictionary<String, object> parameters = new Dictionary<string, object>();
            
            initScript.BranchCallbackWithParams(parameters,null);

            var actualLocation = PlayerSelections.qrPoint;
            Assert.AreEqual(0, actualLocation.x);
            Assert.AreEqual(0, actualLocation.z);
            Assert.AreEqual(0, PlayerSelections.orientation);
            Assert.False(PlayerSelections.qrParametersProvided);
        }
    }
}