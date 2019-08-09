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

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(X)}: {X}, {nameof(Z)}: {Z}, {nameof(Orientation)}: {Orientation}";
        }

        private bool Equals(SyncPoint other)
        {
            return string.Equals(Name, other.Name) && X.Equals(other.X) && Z.Equals(other.Z) && Orientation.Equals(other.Orientation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SyncPoint) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ X.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ Orientation.GetHashCode();
                return hashCode;
            }
        }
    }
}