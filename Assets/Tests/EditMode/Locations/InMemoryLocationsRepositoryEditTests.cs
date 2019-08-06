using Locations;
using Markers;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Locations
{
    public class InMemoryLocationsRepositoryEditTests
    {
        [Test]
        public void GivenMarkerArrayReturnListOfMarkers()
        {
            var locationRepo = new InMemoryLocationsRepository();

            Location[] locations=
            {
                new Location("Chi-Town", "The Deli is Deli-cious"),
                new Location("Des Moines Ville", "File number 2", 5)
            };

            locationRepo.Save(locations);

            Assert.AreEqual(locations, locationRepo.Get());
        }
    }
}