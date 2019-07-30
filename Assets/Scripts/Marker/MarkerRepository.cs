namespace Assets.Scripts.Marker
{
    public interface IMarkerRepository
    {
        global::Assets.Scripts.Marker.Marker[] Get();

        void Save(global::Assets.Scripts.Marker.Marker[] markers);
    }
}
