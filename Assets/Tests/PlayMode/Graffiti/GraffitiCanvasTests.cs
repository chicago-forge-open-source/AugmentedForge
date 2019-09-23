using System;
using System.Threading;
using System.Threading.Tasks;
using Graffiti;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

namespace Tests.PlayMode.Graffiti
{
    public class GraffitiCanvasTests
    {
        private readonly Random _random = new Random();
        private GraffitiCanvas _graffitiCanvas;

        [SetUp]
        public void SetUp()
        {
            _graffitiCanvas = new GraffitiCanvas();
        }

        [Test]
        public void CanGetIoTThing()
        {
            Task.Run(async () => { await _graffitiCanvas.GetIoTThing(); }).GetAwaiter().GetResult();
        }

        [Test]
        public void CanUpdateIoTThing()
        {
            var expectedText = $"Hello World {_random.Next()}";
            Task.Run(async () => { await _graffitiCanvas.UpdateGraffitiCanvasText(expectedText); }).GetAwaiter()
                .GetResult();

            Task.Run(async () =>
            {
                var graffitiCanvasState = await _graffitiCanvas.GetIoTThing();

                var startTime = DateTime.Now;
                while (
                    graffitiCanvasState.text != expectedText
                    && DateTime.Now - startTime < TimeSpan.FromSeconds(2)
                )
                {
                    graffitiCanvasState = await _graffitiCanvas.GetIoTThing();
                    Thread.Yield();
                }

                Assert.AreEqual(expectedText, graffitiCanvasState.text);
            }).GetAwaiter().GetResult();
        }
    }
}