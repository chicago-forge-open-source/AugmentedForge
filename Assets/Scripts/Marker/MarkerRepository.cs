namespace Assets.Scripts.Marker
{
    public interface IMarkerRepository
    {
        global::Marker[] Get();

        void Save(global::Marker[] markers);
    }
}
