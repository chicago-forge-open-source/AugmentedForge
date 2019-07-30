using NUnit.Framework;

namespace Assets.Tests.EditMode.Marker
{
    public class MarkerTests
    {
        public global::Marker Marker = new global::Marker(
            "Test Marker",
            0f,
            1f
        );

        [Test]
        public void TestConstruction()
        {
            Assert.AreEqual("Test Marker",Marker.label);
            Assert.AreEqual(0f,Marker.x);
            Assert.AreEqual(1f,Marker.z);
        }
    }
}