using Assets.Scripts;
using NUnit.Framework;

namespace Assets.Tests.EditMode.Marker
{
    public class InMemoryMarkerRepositoryTests
    {
        [Test]
        public void GivenMarkerArrayReturnListOfMarkers()
        {
            var markerRepo = new InMemoryMarkerRepository();

            global::Assets.Scripts.Marker.Marker[] markerList =
            {
                new global::Assets.Scripts.Marker.Marker("Marker1", 0, 0),
                new global::Assets.Scripts.Marker.Marker("Marker2", 1, 1)
            };

            markerRepo.Save(markerList);

            Assert.AreEqual(markerList, markerRepo.Get());
        }
    }
}