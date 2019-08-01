namespace Roads
{
    public class Road
    {
        public Road(RoadPoint[] points)
        {
            Points = points;
            Tag = "Road";
        }

        public RoadPoint[] Points { get; }

        public string Tag { get; }
    }
}