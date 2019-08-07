using System;
using System.Collections.Generic;
using DataLoaders;
using DefaultNamespace;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class InitializeAppTests
    {
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
        public void GivenStartPointProvided_UserStartPointIsSet()
        {
            var initScript = new GameObject().AddComponent<InitializeApp>();
            
            Dictionary<String, object> paremeters = new Dictionary<string, object>();
            paremeters.Add("z", 1);
            paremeters.Add("x", 1);
            
            initScript.BranchCallbackWithParams(paremeters,null);

            var actualLocation = PlayerSelections._startingPoint;
            Assert.AreEqual(1, actualLocation.x);
            Assert.AreEqual(1, actualLocation.z);
        }
        
        [Test]
        public void GivenNoStartPointProvided_UserStartPointIsNotSet()
        {
            var initScript = new GameObject().AddComponent<InitializeApp>();
            
            Dictionary<String, object> paremeters = new Dictionary<string, object>();
            
            initScript.BranchCallbackWithParams(paremeters,null);

            var actualLocation = PlayerSelections._startingPoint;
            Assert.AreEqual(0, actualLocation.x);
            Assert.AreEqual(0, actualLocation.z);
        }
    }
}