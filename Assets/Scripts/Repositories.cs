using Markers;
using Roads;

namespace Assets.Scripts
{
    public static class Repositories
    {
        public static readonly InMemoryMarkerRepository MarkerRepository = new InMemoryMarkerRepository();
        public static readonly InMemoryRoadRepository RoadRepository = new InMemoryRoadRepository();
    }
}
