public interface IMarkerRepository
{
    Marker[] Get();

    void Save(Marker[] markers);
}
