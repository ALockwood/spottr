using System;
using NpgsqlTypes;
using System.Text.RegularExpressions;

namespace spottr.Models
{
    public class PostGisLocation : ILocation
    {
        public Int32 LocationKey { get; private set; }
        public string Name { get; set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public DateTime DateCreated { get; }
        public DateTime LastUpdated { get; }

        public string Description { get; set; }

        public PostGisLocation(Int32 locationKey, string name, double latitude, double longitude, DateTime created, DateTime updated, string description)
        {
            LocationKey = locationKey;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            DateCreated = created;
            LastUpdated = updated;
            Description = description;
        }

        public PostGisLocation(Int32 locationKey, string name, string point, DateTime created, DateTime updated, string description)
        {
            LocationKey = locationKey;
            Name = name;
            DateCreated = created;
            LastUpdated = updated;
            Description = description;
            ParsePoint(point);
        }

        public void SetLocationLatitudeAndLongitude(string point)
        {
            ParsePoint(point);
        }

        void ParsePoint(string point)
        {
            if (string.IsNullOrWhiteSpace(point))
            {
                Latitude = 0;
                Longitude = 0;
                return;
            }

            try
            {
                string[] latlong = Regex.Match(point, @"([-0-9\x20\.]+)").Value.Split(' ');
                Latitude = double.TryParse(latlong[0], out double lat) ? lat : 0;
                Longitude = double.TryParse(latlong[1], out double lng) ? lng : 0;
            }
            catch (Exception)
            {
                Latitude = 0;
                Longitude = 0;
            }
        }
    }
}
