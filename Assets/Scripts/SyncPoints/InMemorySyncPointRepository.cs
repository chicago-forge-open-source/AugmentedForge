using System;
using System.Collections.Generic;

namespace SyncPoints
{
    public class InMemorySyncPointRepository : SyncPointRepository
    {
        private Dictionary<String, SyncPoint> _syncPoints;
        
        public InMemorySyncPointRepository()
        {
            _syncPoints = new Dictionary<string, SyncPoint>();
        }
        
        public SyncPoint Get(String location)
        {
            return _syncPoints[location];
        }

        public void Save(SyncPoint syncPoint)
        {
            _syncPoints[syncPoint.LocationIdentifier] = syncPoint;
        }
    }
}