using UnityEngine;

namespace Roads
{
    public class RoadBehaviour : MonoBehaviour
    {
        public void Start()
        {
            foreach (var road in Repositories.RoadRepository.Get())
            {
                DrawPathOnLayer(8, road);
                DrawPathOnLayer(9, road);
            }
        }

        private static void DrawPathOnLayer(int layer, Road road)
        {
            var yellowBrickRoad = Instantiate(new GameObject());
            yellowBrickRoad.tag = road.Tag;
            yellowBrickRoad.layer = layer;
            yellowBrickRoad.AddComponent<YellowBrickRoad>().DrawPath(road.Points);
        }
    }
}