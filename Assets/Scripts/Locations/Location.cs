using UnityEngine;

namespace Locations
{
    public class Location
    {
        string name { get; }
        string mapFileName { get; }
        Quaternion rotation { get; }

        public Location(string name, string mapFileName, int zRotation = 0)
        {
            this.name = name;
            this.mapFileName = mapFileName;
            this.rotation = Quaternion.Euler(90, 0, zRotation);
        }
    }
}