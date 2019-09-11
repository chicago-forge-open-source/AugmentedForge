using System.Threading.Tasks;
using Graffiti;
using IoTLights;
using NUnit.Framework;
using UnityEngine;

namespace Tests.PlayMode.IoTLights
{
    public class IoTLightsTests
    {
        private IoTLight _iotLight;

        [SetUp]
        public void SetUp()
        {
            _iotLight = new IoTLight();
        }

        [Test]
        public void CanGetIoTThing()
        {
            Task.Run(async () => { await _iotLight.GetIoTThing(); }).GetAwaiter().GetResult();
        }

        [Test]
        public void CanUpdateIoTThing()
        {
            Task.Run(async () => { await _iotLight.UpdateLightState("on"); }).GetAwaiter().GetResult();
        }
    }
}