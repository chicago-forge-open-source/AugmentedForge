using UnityEngine;

namespace Assets.Scripts.Roads
{
    public class RoadBehaviour : MonoBehaviour
    {
        public GameObject arYellowBrickRoad;
        public GameObject mapYellowBrickRoad;

        public void Start()
        {
            foreach (var road in Repositories.RoadRepository.Get())
            {
                arYellowBrickRoad.GetComponent<YellowBrickRoad>().PathToDraw(road.Points);
                mapYellowBrickRoad.GetComponent<YellowBrickRoad>().PathToDraw(road.Points);
            }
        }
    }
}