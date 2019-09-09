using System.Threading.Tasks;
using Graffiti;
using NUnit.Framework;
using UnityEngine;

namespace Tests.PlayMode.Graffiti
{
    public class WallSquareTests
    {
        private WallSquare _wallSquare;

        [SetUp]
        public void SetUp()
        {
            _wallSquare = new WallSquare();
        }

        [Test]
        public void CanGetIoTThing()
        {
            Task.Run(async () => { await _wallSquare.GetIoTThing(); }).GetAwaiter().GetResult();
        }

        [Test]
        public void CanUpdateIoTThing()
        {
            Task.Run(async () => { await _wallSquare.UpdateMagicWallColor(Color.yellow); }).GetAwaiter().GetResult();
        }
    }
}