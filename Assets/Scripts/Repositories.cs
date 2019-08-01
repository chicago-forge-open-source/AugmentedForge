using Assets.Scripts.Roads;
using Markers;
using SyncPoints;

namespace Assets.Scripts
{
    public static class Repositories
    {
        public static readonly InMemoryMarkerRepository MarkerRepository = new InMemoryMarkerRepository();
        public static readonly InMemoryRoadRepository RoadRepository = new InMemoryRoadRepository();
        public static readonly InMemorySyncPointRepository SyncPointRepository = new InMemorySyncPointRepository();
    }
}
