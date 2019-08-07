using System;
using Locations;
using Markers;
using Roads;
using SyncPoints;
using UnityEngine;

namespace DataLoaders
{
    public class IowaDataLoader : DataLoader
    {
        private const int IowaMapRotation = 28;

        private readonly Marker _crucible = new Marker("Crucible", -10.01f, 18.9f);
        private readonly Marker _focusNorth = new Marker("Focus North", -1.2f, 14.0f);
        private readonly Marker _focusMiddle = new Marker("Focus Middle", 0f, 11.8f);
        private readonly Marker _conferenceNorth = new Marker("Conference North", 2.0f, 8.0f);
        private readonly Marker _kitchen = new Marker("Kitchen", -3.0f, -5.2f);
        private readonly Marker _focusSouth = new Marker("Focus South", 11.0f, -9.0f);
        private readonly Marker _personalHealthRoom = new Marker("Discrete Health Room", 12.2f, -11.3f);
        private readonly Marker _conferenceSouth = new Marker("Conference South", -11.4f, -20.0f);

        private readonly Marker _makersSpace = new Marker("Makers Space", 7.1f, -22.3f);

        private readonly Marker _pathTopLeft = new Marker("", -9.8f, 13.5f);
        private readonly Marker _pathTopRight = new Marker("", -5.8f, 15.6f);
        private readonly Marker _pathCenterLeft = new Marker("", -0.5f, -3.9f);
        private readonly Marker _pathCenterRight = new Marker("", 3.4f, -1.8f);
        private readonly Marker _pathBottomLeft = new Marker("", 4.7f, -13.7f);
        private readonly Marker _pathBottomRight = new Marker("", 8.7f, -11.6f);

        public override void DataLoad()
        {
            Repositories.MarkerRepository.Save(LoadMarkers());
            Repositories.RoadRepository.Save(LoadRoads());
            Repositories.SyncPointRepository.Save(LoadSyncPoints());
            Repositories.LocationsRepository.Save(LoadLocations());
        }


        private protected override Location[] LoadLocations()
        {
            return new[] {new Location("Iowa", "IowaMap", IowaMapRotation)};
        }

        private protected override Marker[] LoadMarkers()
        {
            return new[]
            {
                _crucible, _focusNorth, _focusMiddle, _conferenceNorth, _kitchen,
                _focusSouth, _personalHealthRoom, _conferenceSouth, _makersSpace
            };
        }

        private protected override Road[] LoadRoads()
        {
            var crucibleToMakerSpace = new Road(new[]
            {
                new RoadPoint(0, _crucible),
                new RoadPoint(1, _pathTopLeft),
                new RoadPoint(2, _pathBottomLeft),
                new RoadPoint(3, _makersSpace)
            });

            var crucibleToFocusSouth = new Road(new[]
            {
                new RoadPoint(0, _crucible),
                new RoadPoint(1, _pathTopRight),
                new RoadPoint(2, _pathBottomRight),
                new RoadPoint(3, _focusSouth)
            });

            var kitchenToPersonalHealth = new Road(new[]
            {
                new RoadPoint(0, _kitchen),
                new RoadPoint(1, _pathCenterLeft),
                new RoadPoint(2, _pathCenterRight),
                new RoadPoint(3, _pathBottomRight),
                new RoadPoint(4, _personalHealthRoom)
            });

            return new[] {crucibleToMakerSpace, crucibleToFocusSouth, kitchenToPersonalHealth};
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