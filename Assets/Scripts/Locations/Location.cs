using UnityEngine;

namespace Locations
{
    public class Location
    {
        public string name { get; }
        public string mapFileName { get; }
        public Quaternion rotation { get; }

        public Location(string name, string mapFileName, float zRotation = 0)
        {
            this.name = name;
            this.mapFileName = mapFileName;
            rotation = Quaternion.Euler(90, 0, zRotation);
        }
    }
}