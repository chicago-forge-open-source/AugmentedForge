using System.Threading.Tasks;
using Graffiti;
using NUnit.Framework;
using UnityEngine;

namespace Tests.PlayMode.Graffiti
{
    public class WallSquareTests
    {
        [Test]
        public void CanGetIoTThing()
        {
            Task.Run(async () => { await WallSquare.GetIoTThing(); }).GetAwaiter().GetResult();
        }

        [Test]
        public void CanUpdateIoTThing()
        {
            Task.Run(async () => { await WallSquare.UpdateMagicWallColor(Color.yellow); }).GetAwaiter().GetResult();
        }
    }
}