using NUnit.Framework;
using UnityEngine;

public class YellowBrickRoadEditTests
{
    [Test]
    public void SetPath_SetsRoadPositionPoints()
    {
        var yellowBrick = new GameObject();
        var script = yellowBrick.AddComponent<YellowBrickRoad>();
        var road = yellowBrick.AddComponent<LineRenderer>();
        script.Start();

        var start = NewGameObjectWithPosition(new Vector3(30f, 0, -2));
        var corner1 = NewGameObjectWithPosition(new Vector3(24, 0, -3));
        var corner2 = NewGameObjectWithPosition(new Vector3(20f, 0, -3.5f));
        var end = NewGameObjectWithPosition(new Vector3(10f, 0, -4));

        script.SetPath(new[] {start, corner1, corner2, end});
        
        Assert.AreEqual(4, road.positionCount);
        Assert.AreEqual(start.transform.position, road.GetPosition(0));
        Assert.AreEqual(corner1.transform.position, road.GetPosition(1));
        Assert.AreEqual(corner2.transform.position, road.GetPosition(2));
        Assert.AreEqual(end.transform.position, road.GetPosition(3));
    }

    private static GameObject NewGameObjectWithPosition(Vector3 position)
    {
        var obj = new GameObject();
        obj.transform.position = position;
        return obj;
    }
}