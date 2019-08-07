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

        private readonly Marker _crucible = RotateMarkerBasedOnMapRotation(new Marker("Crucible", 0, 21.4f), IowaMapRotation);
        private readonly Marker _focusNorth = RotateMarkerBasedOnMapRotation(new Marker("Focus North", 5.5f, 12.9f), IowaMapRotation);
        private readonly Marker _focusMiddle = RotateMarkerBasedOnMapRotation(new Marker("Focus Middle", 5.5f, 10.4f), IowaMapRotation);

        private readonly Marker _conferenceNorth =
            RotateMarkerBasedOnMapRotation(new Marker("Conference North", 5.5f, 6.1f), IowaMapRotation);

        private readonly Marker _kitchen = RotateMarkerBasedOnMapRotation(new Marker("Kitchen", -5.1f, -3.2f), IowaMapRotation);
        private readonly Marker _focusSouth = RotateMarkerBasedOnMapRotation(new Marker("Focus South", 5.5f, -13.1f), IowaMapRotation);

        private readonly Marker _personalHealthRoom =
            RotateMarkerBasedOnMapRotation(new Marker("Personal Health Room", 5.5f, -15.7f), IowaMapRotation);

        private readonly Marker _conferenceSouth =
            RotateMarkerBasedOnMapRotation(new Marker("Conference South", 0.7f, -23.0f), IowaMapRotation);

        private readonly Marker _makersSpace =
            RotateMarkerBasedOnMapRotation(new Marker("Makers Space", -4.2f, -23.0f), IowaMapRotation);

        private readonly Marker _pathTopLeft = RotateMarkerBasedOnMapRotation(new Marker("", -2.3f, 16.5f), IowaMapRotation);
        private readonly Marker _pathTopRight = RotateMarkerBasedOnMapRotation(new Marker("", 2.2f, 16.5f), IowaMapRotation);
        private readonly Marker _pathBottomLeft = RotateMarkerBasedOnMapRotation(new Marker("", -2.3f, -14.3f), IowaMapRotation);
        private readonly Marker _pathBottomRight = RotateMarkerBasedOnMapRotation(new Marker("", 2.2f, -14.3f), IowaMapRotation);
        private readonly Marker _pathCenterLeft = RotateMarkerBasedOnMapRotation(new Marker("", -2.3f, -3.2f), IowaMapRotation);
        private readonly Marker _pathCenterRight = RotateMarkerBasedOnMapRotation(new Marker("", 2.2f, -3.2f), IowaMapRotation);

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

        //Made for rotating markers based on map rotation; Use if you make markers 
        private static Marker RotateMarkerBasedOnMapRotation(Marker marker, float mapRotation)
        {
            var x = marker.X;
            var z = marker.Z;

            var vectorLength = VectorLength(x, z);
            var sign = (z / Math.Abs(z));
            var acos = ArcCosInDegrees(x, vectorLength);
            var initialVectorAngle = sign * acos;

            var newVectorAngle = initialVectorAngle + mapRotation;

            var x1 = (float) (vectorLength * CosFromDegrees(newVectorAngle));
            var z1 = (float) (vectorLength * SinFromDegrees(newVectorAngle));

            return new Marker(marker.Label, x1, z1);
        }

        private static double VectorLength(float a, float b)
        {
            return Math.Sqrt(Square(a) + Square(b));
        }

        private static double Square(float num)
        {
            return num * num;
        }

        private static double ArcCosInDegrees(float a, double vectorLength)
        {
            return Math.Acos(a / vectorLength) * (180 / Math.PI);
        }

        private static double CosFromDegrees(double angle)
        {
            return Math.Cos(angle * (Math.PI / 180));
        }

        private static double SinFromDegrees(double angle)
        {
            return Math.Sin(angle * (Math.PI / 180));
        }
    }
}