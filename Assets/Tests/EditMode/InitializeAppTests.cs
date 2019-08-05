using DataLoaders;
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
    }
}