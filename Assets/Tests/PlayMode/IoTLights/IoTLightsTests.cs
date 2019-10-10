using System.Threading.Tasks;
using IoTLights;
using NUnit.Framework;

namespace Tests.PlayMode.IoTLights
{
    public class IoTLightsTests
    {
        private Thing _iotLight;

        [SetUp]
        public void SetUp()
        {
            _iotLight = new Thing("IoTLight");
        }

        [Test]
        public void CanGetIoTThing()
        {
            Task.Run(async () => { await _iotLight.GetThing(); }).GetAwaiter().GetResult();
        }

        [Test]
        public void CanUpdateIoTThing()
        {
            Task.Run(async () => { await _iotLight.UpdateThing("{{ \"state\":\"on\"}}"); }).GetAwaiter().GetResult();
        }
    }
}