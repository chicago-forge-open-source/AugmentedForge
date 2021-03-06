namespace Roads
{
    public class InMemoryRoadRepository : RoadRepository
    {
        private Road[] _roads;

        public InMemoryRoadRepository()
        {
            _roads = new Road[0];
        }

        public Road[] Get()
        {
            return _roads;
        }

        public void Save(Road[] roads)
        {
            _roads = new Road[roads.Length];
            _roads = roads;
        }
    }
}