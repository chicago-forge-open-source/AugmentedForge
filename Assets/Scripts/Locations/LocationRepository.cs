namespace Locations
{
    public interface LocationRepository
    {
        Location[] Get();

        void Save(Location[] locations);
    }
}