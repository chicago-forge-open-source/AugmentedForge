namespace SyncPoints
{
    public class SyncPoint
    {
        public float X { get; }
        public float Z { get; }

        public SyncPoint(string name, float x, float z, float orientation)
        {
            X = x;
            Z = z;
        }
    }
}