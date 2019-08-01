using Markers;
using NUnit.Framework;

namespace Tests.EditMode.Markers
{
    public class MarkerTests
    {
        [Test]
        public void TestConstruction()
        {
            var marker = new Marker("Test Marker", 0f, 1f);
            Assert.AreEqual("Test Marker", marker.Label);
            Assert.AreEqual(0f, marker.X);
            Assert.AreEqual(1f, marker.Z);
            Assert.AreEqual(false, marker.Active);
        }
    }
}