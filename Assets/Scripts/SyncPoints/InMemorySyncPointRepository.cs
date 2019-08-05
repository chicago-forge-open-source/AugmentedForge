namespace SyncPoints
{
    public class InMemorySyncPointRepository : SyncPointRepository
    {
        private SyncPoint[] _syncPoints;
        
        public InMemorySyncPointRepository()
        {
            _syncPoints = new SyncPoint[0];
        }
        
        public SyncPoint[] Get()
        {
            return _syncPoints;
        }

        public void Save(SyncPoint[] syncPoints)
        {
            _syncPoints = new SyncPoint[syncPoints.Length];
            _syncPoints = syncPoints;
        }
    }
}