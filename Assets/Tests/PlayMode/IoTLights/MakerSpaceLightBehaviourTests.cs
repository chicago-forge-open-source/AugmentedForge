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
    public class MakerSpaceLightBehaviourTests
    {

        [SetUp]
        public void SetUp()
        {
            Repositories.LocationsRepository.Save(new[] {new Location("", "ChicagoMap")});
            Repositories.SyncPointRepository.Save(new[] {new SyncPoint("test", 10, 10, 0)});
            SetStateOfLightOnIot("on", "blue");
            SceneManager.LoadScene("ARView");
        }

        [UnityTest]
        public IEnumerator TappingMakerSpaceLightsChangesState()
        {
            yield return null;
            var makerSpaceLights = GameObject.Find("MakerSpaceLights");
            var makerSpaceLightBehaviour = makerSpaceLights.GetComponent<MakerSpaceLightBehaviour>();
            var initialState = makerSpaceLightBehaviour.lightState.on;

            yield return TouchLightOnce(makerSpaceLights, makerSpaceLightBehaviour);
            yield return new WaitUntil(() => makerSpaceLightBehaviour.lightState.on != initialState);

            Assert.AreNotEqual(initialState, makerSpaceLightBehaviour.lightState.on);
        }

        [UnityTest]
        public IEnumerator TappingMakerSpaceLightsTwiceChangesStateBackToOriginal()
        {
            yield return null;
            var makerSpaceLights = GameObject.Find("MakerSpaceLights");
            var makerSpaceLightBehaviour = makerSpaceLights.GetComponent<MakerSpaceLightBehaviour>();
            var initialLightState = makerSpaceLightBehaviour.lightState.on;

            yield return TouchLightOnce(makerSpaceLights, makerSpaceLightBehaviour);

            yield return new WaitUntil(() => makerSpaceLightBehaviour.lightState.on != initialLightState);

            yield return TouchLightOnce(makerSpaceLights, makerSpaceLightBehaviour);
            yield return new WaitUntil(() => makerSpaceLightBehaviour.lightState.on == initialLightState);

            Assert.AreEqual(initialLightState, makerSpaceLightBehaviour.lightState.on);
        }

        private static IEnumerator TouchLightOnce(GameObject light, MakerSpaceLightBehaviour lightBehaviour)
        {
            var position = light.transform.position;
            var touch = new Touch
            {
                position = position, deltaPosition = position, phase = TouchPhase.Began
            };
            lightBehaviour.inputHandler = new MockInputHandler(new List<Touch> {touch});
            lightBehaviour.physicsHandler = new MockPhysicsHandler<MakerSpaceLightBehaviour>
            {
                ValueToReturn = lightBehaviour
            };

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            lightBehaviour.inputHandler = new MockInputHandler(new List<Touch>());
        }

        private static void SetStateOfLightOnIot(string state, string color)
        {
            var light = new Thing("MakerSpaceLights");
            Task.Run(async () => { await light.UpdateThing($"{{ \"state\":\"{state}\", \"color\":\"{color}\"}}"); })
                .GetAwaiter()
                .GetResult();
        }
    }
}