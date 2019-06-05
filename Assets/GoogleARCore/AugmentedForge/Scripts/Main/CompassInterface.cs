namespace GoogleARCore.AugmentedForge.Scripts.Main
{
    public interface ICompassInterface
    {
        bool IsEnabled { get; }
        float TrueHeading { get; }
    }
}