using System.Collections;
using Markers;
using NUnit.Framework;
using Roads;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Roads
{
    public class RoadBehaviourTests
    {
        [UnityTest]
        public IEnumerator Start_WillInitializeArPathAndMapPath()
        {
            var start = new RoadPoint(0, new Marker("Start", 30f, -2));
            var end = new RoadPoint(2, new Marker("End", 10f, -4));
            var corner = new RoadPoint(1, new Marker("Corner 1", 24, -3));
            var mockRoad = new Road(new []{start, corner, end});
            
            Repositories.RoadRepository.Save(new []{mockRoad});
            
            SceneManager.LoadScene("ARView");
            yield return null;
            
            var roads = GameObject.FindGameObjectsWithTag("Road");
            Assert.AreEqual(2, roads.Length);

            foreach (var road in roads)
            {
                var roadRenderer = road.GetComponent<LineRenderer>();
                Assert.AreEqual(3, roadRenderer.positionCount);
                Assert.AreEqual(start.Vector, roadRenderer.GetPosition(0));
                Assert.AreEqual(corner.Vector, roadRenderer.GetPosition(1));
                Assert.AreEqual(end.Vector, roadRenderer.GetPosition(2));
            }
        }
    }
}