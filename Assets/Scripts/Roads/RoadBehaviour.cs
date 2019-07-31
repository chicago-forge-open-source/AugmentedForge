using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Roads
{
    public class RoadBehaviour : MonoBehaviour
    {
        public GameObject YellowBrickRoad;

        public void Start()
        {
            foreach (var road in Repositories.RoadRepository.Get())
            {
                YellowBrickRoad.GetComponent<YellowBrickRoad>().PathToDraw(road.Points);
            }
        }
    }
}