using Markers;
using Roads;
using SyncPoints;

namespace DataLoaders
{
    public class IowaDataLoader : DataLoader
    {
        private readonly Marker _fakePoint1 = new Marker("Fake Point 1", 30.371f, -26.29f);
        private readonly Marker _fakePoint2 = new Marker("Fake Point 2", 30.37f, -3.5f);

        public override void DataLoad()
        {
            Repositories.MarkerRepository.Save(LoadMarkers());
            Repositories.RoadRepository.Save(LoadRoads());
            Repositories.SyncPointRepository.Save(LoadSyncPoints());
        }

        private protected override Marker[] LoadMarkers()
        {
            return new[] {_fakePoint1, _fakePoint2};
        }

        private protected override Road[] LoadRoads()
        {
            var fake1ToFake2 = new Road(new[]
            {
                new RoadPoint(0, _fakePoint1),
                new RoadPoint(1, _fakePoint2)
            });

            return new[] {fake1ToFake2};
        }

        private protected override SyncPoint[] LoadSyncPoints()
        {
            return new[]
            {
                new SyncPoint(0, -1.5f)
            };
        }
    }
}