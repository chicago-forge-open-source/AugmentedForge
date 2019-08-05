namespace SyncPoints
{
    public interface SyncPointRepository
    {
        SyncPoint[] Get();

        void Save(SyncPoint[] syncPoints);
    }
}