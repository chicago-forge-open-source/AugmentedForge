namespace Tests.PlayMode
{
    public class MockCompass : ICompass
    {
        public bool IsEnabled => true;
        public float TrueHeading { get; set; } = 100f;
    }
}