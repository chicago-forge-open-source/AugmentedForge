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
    public class GraffitiCanvasBehaviourTests
    {
        [SetUp]
        public void SetUp()
        {
            Repositories.LocationsRepository.Save(new[] {new Location("", "ChicagoMap")});
            Repositories.SyncPointRepository.Save(new[] {new SyncPoint("test", 10, 10, 0)});
            SetColorOfCanvasOnIoT(Color.blue);
            SceneManager.LoadScene("ARView");
        }

        [UnityTest]
        public IEnumerator GraffitiCanvasGetsColorAndAppliesToSelf()
        {
            yield return null;
            var canvas = GameObject.Find("GraffitiCanvas");
            var canvasColor = canvas.GetComponent<MeshRenderer>().material.color;
            Assert.AreEqual(Color.blue, canvasColor);
        }

        [UnityTest]
        public IEnumerator TappingGraffitiCanvasChangesColorToRed()
        {
            yield return null;
            var canvas = GameObject.Find("GraffitiCanvas");
            var canvasBehaviour = canvas.GetComponent<GraffitiCanvasBehaviour>();

            var position = canvas.transform.position;
            var touch = new Touch
            {
                position = position, deltaPosition = position, phase = TouchPhase.Began
            };
            canvasBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            canvasBehaviour.physicsHandler = new MockPhysicsHandler<GraffitiCanvasBehaviour>
            {
                ValueToReturn = canvasBehaviour
            };

            yield return new WaitForSeconds(2f);
            var canvasColor = canvas.GetComponent<MeshRenderer>().material.color;
            Assert.AreEqual(Color.red, canvasColor);
        }

        private static void SetColorOfCanvasOnIoT(Color color)
        {
            var graffitiCanvas = new GraffitiCanvas();
            Task.Run(async () => { await graffitiCanvas.UpdateGraffitiCanvasColor(color); }).GetAwaiter().GetResult();
        }
    }
}