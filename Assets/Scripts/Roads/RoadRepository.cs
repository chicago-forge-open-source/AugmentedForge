namespace Roads
{
    public interface RoadRepository
    {
        Road[] Get();

        void Save(Road[] markers);
    }
}