using System;
using System.Threading;
using System.Threading.Tasks;
using IoTLights;
using NUnit.Framework;
using Random = System.Random;

namespace Tests.PlayMode.Graffiti
{
    public class IoTMessageWallTests
    {
        private readonly Random _random = new Random();
        private Thing _ioTMessageWall;

        [SetUp]
        public void SetUp()
        {
            _ioTMessageWall = new Thing("Flounder");
        }

        [Test]
        public void CanGetIoTThing()
        {
            Task.Run(async () => { await _ioTMessageWall.GetThing(); }).GetAwaiter().GetResult();
        }

        [Test]
        public void CanUpdateIoTThing()
        {
            var expectedText = $"Hello World {_random.Next()}";
            Task.Run(async () => { await _ioTMessageWall.UpdateThing($"{{ \"text\":\"{expectedText}\"}}"); }).GetAwaiter()
                .GetResult();

            Task.Run(async () =>
            {
                var graffitiCanvasState = await _ioTMessageWall.GetThing();

                var startTime = DateTime.Now;
                while (
                    graffitiCanvasState.text != expectedText
                    && DateTime.Now - startTime < TimeSpan.FromSeconds(2)
                )
                {
                    graffitiCanvasState = await _ioTMessageWall.GetThing();
                    Thread.Yield();
                }

                Assert.AreEqual(expectedText, graffitiCanvasState.text);
            }).GetAwaiter().GetResult();
        }
    }
}