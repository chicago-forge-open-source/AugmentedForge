namespace SyncPoints
{
    public class SyncPoint
    {
        public string Name { get; }
        public float X { get; }
        public float Z { get; }
        public float Orientation { get; }

        public SyncPoint(string name, float x, float z, float orientation)
        {
            Name = name;
            X = x;
            Z = z;
            Orientation = orientation;
        }
    }
}