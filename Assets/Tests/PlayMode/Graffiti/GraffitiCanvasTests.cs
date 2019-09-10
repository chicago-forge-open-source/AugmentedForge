using System.Threading.Tasks;
using Graffiti;
using NUnit.Framework;
using UnityEngine;

namespace Tests.PlayMode.Graffiti
{
    public class GraffitiCanvasTests
    {
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
            Task.Run(async () => { await _graffitiCanvas.UpdateGraffitiCanvasColor(Color.yellow); }).GetAwaiter().GetResult();
        }
    }
}