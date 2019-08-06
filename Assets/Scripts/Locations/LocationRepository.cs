namespace Locations
{
    public interface LocationRepository
    {
        Location[] Get();

        Location GetLocationByName();

        void Save(Location[] locations);
    }
}