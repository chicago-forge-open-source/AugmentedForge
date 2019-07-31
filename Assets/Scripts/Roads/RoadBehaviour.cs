using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Roads
{
    public class RoadBehaviour : MonoBehaviour
    {
        public GameObject YellowBrickRoad;

        public void Start()
        {
            var roads = Repositories.RoadRepository.Get();
            if (roads == null) return;
            foreach (var road in roads)
            {
                YellowBrickRoad.GetComponent<YellowBrickRoad>().PathToDraw(road.Points);
            }
        }
    }
}