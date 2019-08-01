using Markers;
using NUnit.Framework;

namespace Tests.EditMode.Markers
{
    public class InMemoryMarkerRepositoryTests
    {
        [Test]
        public void GivenMarkerArrayReturnListOfMarkers()
        {
            var markerRepo = new InMemoryMarkerRepository();

            Marker[] markerList =
            {
                new Marker("Marker1", 0, 0),
                new Marker("Marker2", 1, 1)
            };

            markerRepo.Save(markerList);

            Assert.AreEqual(markerList, markerRepo.Get());
        }
    }
}