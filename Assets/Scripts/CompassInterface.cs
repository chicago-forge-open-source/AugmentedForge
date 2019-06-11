namespace AugmentedForge
{
    public interface ICompass
    {
        bool IsEnabled { get; }
        float TrueHeading { get; }
    }
}
