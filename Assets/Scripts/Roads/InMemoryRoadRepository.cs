namespace Assets.Scripts.Roads
{
    public class InMemoryRoadRepository: IRoadRepository
    {
        private Road[] _roads;
        
        public Road[] Get()
        {
            return _roads;
        }

        public void Save(Road[] roads)
        {
            _roads = roads;
        }
    }
}