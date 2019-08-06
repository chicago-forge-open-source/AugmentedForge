using Locations;
using Markers;
using Roads;
using SyncPoints;

namespace DataLoaders
{
    public abstract class DataLoader
    {
        public abstract void DataLoad();
        private protected abstract Marker[] LoadMarkers();
        private protected abstract Road[] LoadRoads();
        private protected abstract SyncPoint[] LoadSyncPoints();
        private protected abstract Location[] LoadLocations();
    }
}