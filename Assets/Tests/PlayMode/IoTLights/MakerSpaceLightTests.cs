using System.Threading.Tasks;
using IoTLights;
using NUnit.Framework;

namespace Tests.PlayMode.IoTLights
{
    public class MakerSpaceLightTests
    {
        private MakerSpaceLight _makerSpaceLight;

        [SetUp]
        public void SetUp()
        {
            _makerSpaceLight = new MakerSpaceLight();
        }

        [Test]
        public void CanGetIoTThing()
        {
            Task.Run(async () => { await _makerSpaceLight.GetIoTThing(); }).GetAwaiter().GetResult();
        }

        [Test]
        public void CanUpdateIoTThing()
        {
            Task.Run(async () => { await _makerSpaceLight.UpdateLightState("on"); }).GetAwaiter().GetResult();
        }
    }
}