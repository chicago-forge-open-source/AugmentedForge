namespace Assets.Scripts
{
    public interface ICompass
    {
        bool IsEnabled { get; }
        float TrueHeading { get; }
    }
}
