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
        private GameObject _makerSpaceLights;
        private MakerSpaceLightBehaviour _makerSpaceLightBehaviour;

        [SetUp]
        public void SetUp()
        {
            Repositories.LocationsRepository.Save(new[] {new Location("", "ChicagoMap")});
            Repositories.SyncPointRepository.Save(new[] {new SyncPoint("test", 10, 10, 0)});
            SetStateOfLightOnIot("on", "blue");
            SceneManager.LoadScene("ARView");
            var unused = SetGameObjAndBehaviour();
        }

        private IEnumerator SetGameObjAndBehaviour()
        {
            yield return null;
            _makerSpaceLights = GameObject.Find("MakerSpaceLights");
            _makerSpaceLightBehaviour = _makerSpaceLights.GetComponent<MakerSpaceLightBehaviour>();
        }

        [UnityTest]
        public IEnumerator TappingMakerSpaceLightsChangesState()
        {
            var initialState = _makerSpaceLightBehaviour.lightState.on;

            yield return TouchLightOnce(_makerSpaceLights, _makerSpaceLightBehaviour);
            yield return new WaitUntil(() => _makerSpaceLightBehaviour.lightState.on != initialState);

            Assert.AreNotEqual(initialState, _makerSpaceLightBehaviour.lightState.on);
        }

        [UnityTest]
        public IEnumerator TappingMakerSpaceLightsTwiceChangesStateBackToOriginal()
        {
            var initialLightState = _makerSpaceLightBehaviour.lightState.on;

            yield return TouchLightOnce(_makerSpaceLights, _makerSpaceLightBehaviour);

            yield return new WaitUntil(() => _makerSpaceLightBehaviour.lightState.on != initialLightState);

            yield return TouchLightOnce(_makerSpaceLights, _makerSpaceLightBehaviour);
            yield return new WaitUntil(() => _makerSpaceLightBehaviour.lightState.on == initialLightState);

            Assert.AreEqual(initialLightState, _makerSpaceLightBehaviour.lightState.on);
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