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
    public class MessageWallBehaviourTests
    {
        [SetUp]
        public void SetUp()
        {
            Repositories.LocationsRepository.Save(new[] {new Location("", "ChicagoMap")});
            Repositories.SyncPointRepository.Save(new[] {new SyncPoint("test", 10, 10, 0)});
            SceneManager.LoadScene("ARView");
        }
        
        [UnityTest]
        [UnityPlatform(RuntimePlatform.Android, RuntimePlatform.IPhonePlayer)]
        public IEnumerator OnTouchKeyboardOpens()
        {
            yield return null;
            var canvas = GameObject.Find("GraffitiCanvas");
            var canvasBehaviour = canvas.GetComponent<MessageWallBehaviour>();

            yield return TouchGraffitiCanvasOnce(canvas, canvasBehaviour);

            Assert.AreEqual(TouchScreenKeyboard.Status.Visible,canvasBehaviour.keyboard.status);
        }

        [UnityTest]
        public IEnumerator OnUserSubmittingTextGraffitiCanvasChangesText()
        {
            yield return null;
            var canvas = GameObject.Find("GraffitiCanvas");
            var canvasBehaviour = canvas.GetComponent<MessageWallBehaviour>();
            var initialText = "This is fun";

            yield return TouchGraffitiCanvasOnce(canvas, canvasBehaviour);
            canvasBehaviour.keyboard.text = initialText;
            
            yield return new WaitForSeconds(2f);
            
            Assert.AreEqual(initialText, canvasBehaviour.canvasText.text);
        }

        private static IEnumerator TouchGraffitiCanvasOnce(GameObject canvas, MessageWallBehaviour canvasBehaviour)
        {
            var position = canvas.transform.position;
            var touch = new Touch
            {
                position = position, deltaPosition = position, phase = TouchPhase.Began
            };
            canvasBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            canvasBehaviour.physicsHandler = new MockPhysicsHandler<MessageWallBehaviour>
            {
                ValueToReturn = canvasBehaviour
            };
            
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            canvasBehaviour.inputHandler = new MockInputHandler(new List<Touch>());
        }
    }
}