using System.Threading.Tasks;
using DefaultNamespace;
using NUnit.Framework;

namespace Tests.PlayMode
{
    public class WallSquareTests
    {
        [Test]
        public void WallSquareTestsSimplePasses()
        {
            Task.Run(async () => { await WallSquare.DoIoTThing(); }).GetAwaiter().GetResult();
        }
    }
}