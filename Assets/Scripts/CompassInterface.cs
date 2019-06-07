namespace AugmentedForge
{
    public interface ICompassInterface
    {
        bool IsEnabled { get; }
        float TrueHeading { get; }
    }
}
