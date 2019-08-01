namespace Assets.Scripts.Markers
{
    public class Marker
    {
        public string Label { get; }
        public float X { get; }
        public float Z { get; }
        public bool Active { get; set; }

        public Marker(string label, float x, float z)
        {
            Label = label;
            X = x;
            Z = z;
            Active = false;
        }
    }
}