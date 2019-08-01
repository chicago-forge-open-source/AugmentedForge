namespace SyncPoints
{
    public class SyncPoint
    {
        public string LocationIdentifier { get; }
        public float X { get; }
        public float Z { get; }
        public bool Active { get; set; }

        public SyncPoint(string locationIdentifier, float x, float z)
        {
            LocationIdentifier = locationIdentifier;
            X = x;
            Z = z;
            Active = false;
        }
    }
}