using UnityEngine;

namespace Locations
{
    public class Location
    {
        public string name { get; }
        public string mapFileName { get; }
        public Quaternion rotation { get; }

        public Location(string name, string mapFileName, int zRotation = 0)
        {
            this.name = name;
            this.mapFileName = mapFileName;
            this.rotation = Quaternion.Euler(90, 0, zRotation);
        }
    }
}