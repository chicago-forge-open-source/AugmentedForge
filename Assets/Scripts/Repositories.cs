using Markers;
using SyncPoints;
using Roads;

public static class Repositories
{
        public static readonly InMemoryMarkerRepository MarkerRepository = new InMemoryMarkerRepository();
        public static readonly InMemoryRoadRepository RoadRepository = new InMemoryRoadRepository();
        public static readonly InMemorySyncPointRepository SyncPointRepository = new InMemorySyncPointRepository();
}
