using System;

namespace SyncPoints
{
    public interface SyncPointRepository
    {
        SyncPoint Get(String location);

        void Save(SyncPoint syncPoint);
    }
}