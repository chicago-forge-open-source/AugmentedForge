namespace Main
{
    public interface ICompassInterface
    {
        bool IsEnabled { get; }
        float TrueHeading { get; }
    }
}
