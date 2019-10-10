using System.Threading.Tasks;
using IoTLights;
using NUnit.Framework;

namespace Tests.PlayMode.IoTLights
{
    public class MakerSpaceLightTests
    {
        private Thing _makerSpaceLights;

        [SetUp]
        public void SetUp()
        {
            _makerSpaceLights = new Thing("MakerSpaceLights");
        }

        [Test]
        public void CanGetIoTThing()
        {
            Task.Run(async () => { await _makerSpaceLights.GetThing(); }).GetAwaiter().GetResult();
        }

        [Test]
        public void CanUpdateIoTThing()
        {
            Task.Run(async () => { await _makerSpaceLights.UpdateThing("{{ \"state\":\"on\"}}"); }).GetAwaiter().GetResult();
        }
    }
}