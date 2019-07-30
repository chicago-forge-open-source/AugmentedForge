namespace Assets.Scripts
{
    public static class DataLoader
    {
        public static void DataLoad()
        {
            var markers = LoadMarkers();
            Repositories.MarkerRepository.Save(markers);
        }

        private static Marker.Marker[] LoadMarkers()
        {
            var makerSpace = new Marker.Marker(
                "Maker Space",
                30.371f,
                -26.29f
            );

            var crucible = new Marker.Marker(
                "Crucible",
                27.5f,
                -11.44f
            );

            var focusRoom = new Marker.Marker(
                "Focus Room",
                27.5f,
                -8.03f
            );

            var kitchen = new Marker.Marker(
                "Kitchen",
                43.175f,
                -1.144f
            );

            var greaterMHub = new Marker.Marker(
                "Greater MHub",
                28.875f,
                9.262001f
            );

            return new[] {makerSpace, crucible, focusRoom, kitchen, greaterMHub};
        }
    }
}