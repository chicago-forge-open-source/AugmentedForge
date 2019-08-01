using Markers;
using NUnit.Framework;

namespace Assets.Tests.EditMode.Markers
{
    public class MarkerTests
    {
        public Marker Marker = new Marker(
            "Test Marker",
            0f,
            1f
        );

        [Test]
        public void TestConstruction()
        {
            Assert.AreEqual("Test Marker",Marker.Label);
            Assert.AreEqual(0f,Marker.X);
            Assert.AreEqual(1f,Marker.Z);
            Assert.AreEqual(false, Marker.Active);
        }
    }
}