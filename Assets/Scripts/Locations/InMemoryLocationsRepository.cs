namespace Locations
{
    public class InMemoryLocationsRepository: LocationRepository
    {
        private Location[] _locations = new Location[0];


        public Location[] Get()
        {
            return _locations;
        }

        public void Save(Location[] locations)
        {
            _locations = new Location[locations.Length];
            _locations = locations;
        }
    }
}