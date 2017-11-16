namespace spottr.Models
{
    public sealed class LocationPinForPosting
    {
        public string Name { get; }
        public double Latitude { get; }
        public double Longitude { get; }
        public string Description { get; }

        public LocationPinForPosting(LocationPin location)
        {
            Name = location.Name;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
            Description = location.Description;
        }
    }
}
