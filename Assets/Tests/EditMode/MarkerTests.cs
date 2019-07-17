using NUnit.Framework;
using NUnit.Framework.Internal;

public class MarkerTests
{
    public Marker Marker = new Marker(
        "Test Marker",
        0f,
        1f
    );

    [Test]
    public void TestConsturction()
    {
        Assert.AreEqual("Test Marker",Marker.label);
        Assert.AreEqual(0f,Marker.x);
        Assert.AreEqual(1f,Marker.z);
    }
}