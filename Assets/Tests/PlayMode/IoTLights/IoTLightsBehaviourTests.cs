using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IoTLights;
using Locations;
using NUnit.Framework;
using SyncPoints;
using Tests.Mocks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode.IoTLights
{
    public class IoTLightsBehaviourTests
    {
        [SetUp]
        public void SetUp()
        {
            Repositories.LocationsRepository.Save(new[] {new Location("", "ChicagoMap")});
            Repositories.SyncPointRepository.Save(new[] {new SyncPoint("test", 10, 10, 0)});
            SetStateOfLightOnIot("on");
            SceneManager.LoadScene("ARView");
        }

        [UnityTest]
        public IEnumerator IoTLightGetsStateAndAppliesToSelf()
        {
            yield return null;
            var light = GameObject.Find("IoTLight");
            var lightColor = light.GetComponent<MeshRenderer>().material.color;
            Assert.AreEqual(Color.yellow, lightColor);
        }

        [UnityTest]
        public IEnumerator TappingIoTLightChangesState()
        {
            yield return null;
            var light = GameObject.Find("IoTLight");
            var lightBehaviour = light.GetComponent<IoTLightBehaviour>();
            var initialState = lightBehaviour.onOffState;
            
            yield return TouchIoTLightOnce(light, lightBehaviour);
            
            yield return new WaitForSeconds(2f);
            
            Assert.AreNotEqual(initialState, lightBehaviour.onOffState);
        }
        
        [UnityTest]
        public IEnumerator TappingIoTLightTwiceChangesStateBackToOriginal()
        {
            yield return null;
            var light = GameObject.Find("IoTLight");
            var lightBehaviour = light.GetComponent<IoTLightBehaviour>();
            var initialLightState = lightBehaviour.onOffState;

            yield return TouchIoTLightOnce(light, lightBehaviour);
            yield return new WaitForSeconds(2f);
            
            yield return TouchIoTLightOnce(light, lightBehaviour);
            yield return new WaitForSeconds(2f);
            
            Assert.AreEqual(initialLightState, lightBehaviour.onOffState);
        }

        private static IEnumerator TouchIoTLightOnce(GameObject light, IoTLightBehaviour lightBehaviour)
        {
            var position = light.transform.position;
            var touch = new Touch
            {
                position = position, deltaPosition = position, phase = TouchPhase.Began
            };
            lightBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            lightBehaviour.physicsHandler = new MockPhysicsHandler<IoTLightBehaviour>
            {
                ValueToReturn = lightBehaviour
            };
            
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            lightBehaviour.inputHandler = new MockInputHandler(new List<Touch>());
        }

        private static void SetStateOfLightOnIot(string state)
        {
            var light = new IoTLight();
            Task.Run(async () => { await light.UpdateLightState(state); }).GetAwaiter().GetResult();
        }
    }
}