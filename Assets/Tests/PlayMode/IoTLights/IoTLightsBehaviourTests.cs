using System;
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
        public IEnumerator TappingIoTLightChangesState()
        {
            yield return null;
            var light = GameObject.Find("IoTLight");
            var lightBehaviour = light.GetComponent<IoTLightBehaviour>();
            var lightSwitch = light.GetComponent<IoTLightBehaviour>().lightSwitch;
            lightSwitch.transform.rotation = Quaternion.Euler(0,0,0);

            var initialState = lightBehaviour.onOffState;
            var initialZRotation = lightSwitch.transform.rotation.y;

            yield return TouchIoTLightOnce(light, lightBehaviour);
            yield return new WaitUntil(() => lightBehaviour.onOffState != initialState);

            Assert.AreNotEqual(initialState, lightBehaviour.onOffState);
            Assert.AreNotEqual(initialZRotation, lightSwitch.transform.rotation.y);
        }

        [UnityTest]
        public IEnumerator IoTLightGetsStateAndAppliesToSelf()
        {
            yield return null;
            var light = GameObject.Find("IoTLight");
            var lightSwitch = light.GetComponent<IoTLightBehaviour>().lightSwitch;
            lightSwitch.transform.rotation = Quaternion.Euler(0,0,0);

            yield return new WaitUntil(() => Math.Abs(lightSwitch.transform.rotation.eulerAngles.y - 180) < 0.01);

            Assert.AreEqual(180, lightSwitch.transform.rotation.eulerAngles.y);
        }

        [UnityTest]
        public IEnumerator TappingIoTLightTwiceChangesStateBackToOriginal()
        {
            yield return null;
            var light = GameObject.Find("IoTLight");
            var lightBehaviour = light.GetComponent<IoTLightBehaviour>();
            var lightSwitch = lightBehaviour.lightSwitch;
            lightSwitch.transform.rotation = Quaternion.Euler(0,0,0);

            var initialLightState = lightBehaviour.onOffState;
            var initialZRotation = lightSwitch.transform.rotation.y;

            yield return TouchIoTLightOnce(light, lightBehaviour);

            yield return new WaitUntil(() => lightBehaviour.onOffState != initialLightState);

            yield return TouchIoTLightOnce(light, lightBehaviour);
            yield return new WaitUntil(() => lightBehaviour.onOffState == initialLightState);

            Assert.AreEqual(initialLightState, lightBehaviour.onOffState);
            Assert.AreEqual(initialZRotation, lightSwitch.transform.rotation.y);
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
            var light = new Thing("IoTLight");
            Task.Run(async () => { await light.UpdateThing($"{{ \"state\":\"{state}\"}}"); })
                .GetAwaiter()
                .GetResult();
        }
    }
}