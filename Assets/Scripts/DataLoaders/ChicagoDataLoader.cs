﻿using Markers;
using SyncPoints;
using Roads;


namespace DataLoaders
{
    public class ChicagoDataLoader : DataLoader
    {
        private readonly Marker _makerSpace = new Marker("Maker Space", 30.371f, -26.29f);
        private readonly Marker _entrance = new Marker("Entrance", 30.37f, -3.5f);
        private readonly Marker _kitchen = new Marker("Kitchen", 43.175f, -3.5f);
        private readonly Marker _crucible = new Marker("Crucible", 27.5f, -11.44f);
        private readonly Marker _focusRoom = new Marker("Focus Room", 27.5f, -8.03f);
        private readonly Marker _greaterMHub = new Marker("Greater MHub", 28.875f, 9.262001f);

        public override void DataLoad()
        {
            Repositories.MarkerRepository.Save(LoadMarkers());
            Repositories.RoadRepository.Save(LoadRoads());
            Repositories.SyncPointRepository.Save(LoadSyncPoints());
        }

        private protected override Marker[] LoadMarkers()
        {
            return new[] {_makerSpace, _crucible, _focusRoom, _kitchen, _greaterMHub, _entrance};
        }

        private protected override Road[] LoadRoads()
        {
            var makerToKitchen = new Road(new[]
            {
                new RoadPoint(0, _makerSpace),
                new RoadPoint(1, _entrance),
                new RoadPoint(2, _kitchen)
            });

            var makerToGreaterMHub = new Road(new[]
            {
                new RoadPoint(0, _makerSpace),
                new RoadPoint(1, _entrance),
                new RoadPoint(2, _greaterMHub)
            });

            return new[] {makerToKitchen, makerToGreaterMHub};
        }


        private protected override SyncPoint[] LoadSyncPoints()
        {
            return new[]
            {
                new SyncPoint(26.94955f, -18.17933f)
            };
        }
    }
}