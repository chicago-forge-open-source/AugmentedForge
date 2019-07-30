using NUnit.Framework;

namespace Assets.Tests.EditMode.Marker
{
    public class InMemoryMarkerRepositoryTests
    {
        [Test]
        public void GivenMarkerArrayReturnListOfMarkers()
        {
            var markerRepo = new InMemoryMarkerRepository();

            global::Marker[] markerList =
            {
                new global::Marker("Marker1", 0, 0),
                new global::Marker("Marker2", 1, 1)
            };

            markerRepo.Save(markerList);

            Assert.AreEqual(markerList, markerRepo.Get());
        }
    }
}