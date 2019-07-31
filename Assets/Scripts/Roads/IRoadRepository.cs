namespace Assets.Scripts.Roads
{
    public interface IRoadRepository
    {
        Road[] Get();

        void Save(Road[] markers);
    }
}