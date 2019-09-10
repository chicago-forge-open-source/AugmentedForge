using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Graffiti;
using Locations;
using NUnit.Framework;
using SyncPoints;
using Tests.Mocks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Graffiti
{
    public class MagicWallBehaviourTests
    {
        [SetUp]
        public void SetUp()
        {
            Repositories.LocationsRepository.Save(new[] {new Location("", "ChicagoMap")});
            Repositories.SyncPointRepository.Save(new[] {new SyncPoint("test", 10, 10, 0)});
            SetColorOfWallOnIoT(Color.blue);
            SceneManager.LoadScene("ARView");
        }

        [UnityTest]
        public IEnumerator MagicWallGetsColorAndAppliesToSelf()
        {
            yield return null;
            var wall = GameObject.Find("MagicWall");
            var wallColor = wall.GetComponent<MeshRenderer>().material.color;
            Assert.AreEqual(Color.blue, wallColor);
        }

        [UnityTest]
        public IEnumerator TappingGraffitiCanvasChangesColorToRed()
        {
            yield return null;
            var wall = GameObject.Find("MagicWall");
            var wallBehaviour = wall.GetComponent<MagicWallBehaviour>();

            var position = wall.transform.position;
            var touch = new Touch
            {
                position = position, deltaPosition = position, phase = TouchPhase.Began
            };
            wallBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            wallBehaviour.physicsHandler = new MockPhysicsHandler<MagicWallBehaviour>
            {
                ValueToReturn = wallBehaviour
            };
            wallBehaviour.Update();
            yield return null;
            yield return null;
            var wallColor = wall.GetComponent<MeshRenderer>().material.color;
            Assert.AreEqual(Color.red, wallColor);
        }

        private static void SetColorOfWallOnIoT(Color color)
        {
            var graffitiCanvas = new GraffitiCanvas();
            Task.Run(async () => { await graffitiCanvas.UpdateMagicWallColor(color); }).GetAwaiter().GetResult();
        }
    }
}