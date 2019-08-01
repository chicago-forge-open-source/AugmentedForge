using Markers;
using NUnit.Framework;
using Roads;

namespace Tests.EditMode.Roads
{
    public class RoadTests
    {
        [Test]
        public void GivenMarkerArrayReturnListOfMarkers()
        {
            var point1 = new RoadPoint(0, new Marker("Thing1", 20, -2));
            var point2 = new RoadPoint(0, new Marker("Thing2", 20, -8));
            var road = new Road(new[] {point1, point2});
            
            Assert.AreEqual("Road", road.Tag);
            Assert.AreEqual(2, road.Points.Length);
            Assert.AreEqual(point1, road.Points[0]);
            Assert.AreEqual(-1, point1.Vector.y);
            Assert.AreEqual(point2, road.Points[1]);
            Assert.AreEqual(-1,point2.Vector.y);
        }
    }
}
