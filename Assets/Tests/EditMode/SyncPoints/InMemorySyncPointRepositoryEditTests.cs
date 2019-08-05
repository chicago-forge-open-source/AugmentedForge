using NUnit.Framework;
using SyncPoints;

namespace Tests.EditMode.SyncPoints
{
    public class InMemorySyncPointRepositoryEditTests
    {
        private InMemorySyncPointRepository _inMemorySyncPointRepository;

        [SetUp]
        public void Setup()
        {
            _inMemorySyncPointRepository = new InMemorySyncPointRepository();
        }

        [Test]
        public void Save_ASavedSyncPointCanBeRetrieved()
        {
            var syncPoint = new SyncPoint(2, 2);

            _inMemorySyncPointRepository.Save(new[] {syncPoint});

            var result = _inMemorySyncPointRepository.Get();

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(2, result[0].X);
            Assert.AreEqual(2, result[0].Z);
        }

        [Test]
        public void Save_CanSaveMultipleSyncPoints()
        {
            var syncPoint1 = new SyncPoint(1, 1);
            var syncPoint2 = new SyncPoint(2, 2);

            _inMemorySyncPointRepository.Save(new[] {syncPoint1, syncPoint2});

            var result = _inMemorySyncPointRepository.Get();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0].X);
            Assert.AreEqual(1, result[0].Z);
            Assert.AreEqual(2, result[1].X);
            Assert.AreEqual(2, result[1].Z);
        }
    }
}