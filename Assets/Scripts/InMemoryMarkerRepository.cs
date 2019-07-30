using Assets.Scripts.Marker;

namespace Assets.Scripts
{
    public class InMemoryMarkerRepository : IMarkerRepository
    {
        private Marker.Marker[] _markers;

        public InMemoryMarkerRepository()
        {
            _markers = new Marker.Marker[0];
        }

        public Marker.Marker[] Get()
        {
            return _markers;
        }

        public void Save(Marker.Marker[] markers)
        {
            _markers = new Marker.Marker[markers.Length];
            _markers = markers;
        }
    }
}