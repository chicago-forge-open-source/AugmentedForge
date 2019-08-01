using NUnit.Framework;
using SyncPoints;

namespace Tests.EditMode.SyncPoints
{
    public class InMemorySyncPointRepostoryEditTests
    {
        private InMemorySyncPointRepository _inMemorySyncPointRepository;
        
        [SetUp]
        public void Setup()
        {
           _inMemorySyncPointRepository = new InMemorySyncPointRepository();
        }
        
        [Test]
        public void Save_ASavedSyncPointCanBeGot()
        {
            var syncPoint = new SyncPoint("test", 2,2);
            
            _inMemorySyncPointRepository.Save(syncPoint);

            var result = _inMemorySyncPointRepository.Get("test");
            
            Assert.AreEqual("test", result.LocationIdentifier);
            Assert.AreEqual(2, result.X);
            Assert.AreEqual(2, result.Z);
        }
        
        [Test]
        public void Save_TwoSyncPointsWithTheSameNameAreSaved_TheLaterOverwritesThePrevious()
        {
            var syncPoint1 = new SyncPoint("test", 1,1);
            var syncPoint2 = new SyncPoint("test", 2,2);
            
            _inMemorySyncPointRepository.Save(syncPoint1);
            _inMemorySyncPointRepository.Save(syncPoint2);

            var result = _inMemorySyncPointRepository.Get("test");
            
            Assert.AreEqual("test", result.LocationIdentifier);
            Assert.AreEqual(2, result.X);
            Assert.AreEqual(2, result.Z);
        }
    }
}