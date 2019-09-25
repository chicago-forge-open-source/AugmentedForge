using System;
using System.Threading;
using System.Threading.Tasks;
using Graffiti;
using NUnit.Framework;
using Random = System.Random;

namespace Tests.PlayMode.Graffiti
{
    public class IoTMessageWallTests
    {
        private readonly Random _random = new Random();
        private IoTMessageWall _ioTMessageWall;

        [SetUp]
        public void SetUp()
        {
            _ioTMessageWall = new IoTMessageWall();
        }

        [Test]
        public void CanGetIoTThing()
        {
            Task.Run(async () => { await _ioTMessageWall.GetIoTThing(); }).GetAwaiter().GetResult();
        }

        [Test]
        public void CanUpdateIoTThing()
        {
            var expectedText = $"Hello World {_random.Next()}";
            Task.Run(async () => { await _ioTMessageWall.UpdateMessageWallText(expectedText); }).GetAwaiter()
                .GetResult();

            Task.Run(async () =>
            {
                var graffitiCanvasState = await _ioTMessageWall.GetIoTThing();

                var startTime = DateTime.Now;
                while (
                    graffitiCanvasState.text != expectedText
                    && DateTime.Now - startTime < TimeSpan.FromSeconds(2)
                )
                {
                    graffitiCanvasState = await _ioTMessageWall.GetIoTThing();
                    Thread.Yield();
                }

                Assert.AreEqual(expectedText, graffitiCanvasState.text);
            }).GetAwaiter().GetResult();
        }
    }
}