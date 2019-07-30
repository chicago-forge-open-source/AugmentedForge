using Assets.Scripts.Markers;

namespace Assets.Scripts
{
    public class InMemoryMarkerRepository : IMarkerRepository
    {
        private Marker[] _markers;

        public InMemoryMarkerRepository()
        {
            _markers = new Marker[0];
        }

        public Marker[] Get()
        {
            return _markers;
        }

        public void Save(Marker[] markers)
        {
            _markers = new Marker[markers.Length];
            _markers = markers;
        }
    }
}