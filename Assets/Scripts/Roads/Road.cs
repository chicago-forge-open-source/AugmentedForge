namespace Assets.Scripts.Roads
{
    public class Road
    {
        public Road(RoadPoint[] points)
        {
            Points = points;
        }

        public RoadPoint[] Points { get; }
    }
}