using System.Collections;
using System.Threading.Tasks;
using Graffiti;
using Locations;
using NUnit.Framework;
using SyncPoints;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Graffiti
{
    public class MagicWallBehaviourTests
    {
        [UnityTest]
        public IEnumerator MagicWallGetsColorAndAppliesToSelf()
        {
            Repositories.LocationsRepository.Save(new[] {new Location("", "ChicagoMap")});
            Repositories.SyncPointRepository.Save(new[] {new SyncPoint("test", 10, 10, 0)});
            SetColorOfWallOnIoT(Color.blue);
            SceneManager.LoadScene("ARView");
            yield return null;
            var wall = GameObject.Find("MagicWall");
            var wallColor = wall.GetComponent<MeshRenderer>().material.color;
            Assert.AreEqual(Color.blue, wallColor);
        }

        private void SetColorOfWallOnIoT(Color color)
        {
            Task.Run(async () => { await WallSquare.UpdateMagicWallColor(color); }).GetAwaiter().GetResult();
        }
    }
}