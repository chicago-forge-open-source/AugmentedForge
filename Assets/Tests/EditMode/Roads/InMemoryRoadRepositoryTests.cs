using Markers;
using NUnit.Framework;
using Roads;

namespace Tests.EditMode.Roads
{
    public class InMemoryRoadRepositoryTests
    {
        [Test]
        public void GivenMarkerArrayReturnListOfMarkers()
        {
            var roadRepo = new InMemoryRoadRepository();
            var point1 = new RoadPoint(0, new Marker("#1", 10, -4));
            var point2 = new RoadPoint(4, new Marker("#2", 30, -2));

            Road[] roadList =
            {
                new Road(new[] {point1}),
                new Road(new[] {point2})
            };

            roadRepo.Save(roadList);

            Assert.AreEqual(roadList, roadRepo.Get());
        }
    }
}