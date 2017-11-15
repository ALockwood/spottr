using System;
using NpgsqlTypes;

namespace spottr.Models
{
    public class PostGisLocation : ILocation
    {
        public Int32 LocationKey { get; private set; }
        public string Name { get; set; }
        public string LocationPoint { get; set; }

        public DateTime DateCreated { get; }
        public DateTime LastUpdated { get; }

        public string Description { get; set; }

        public PostGisLocation(Int32 locationKey, string name, string point, DateTime created, DateTime updated, string description)
        {
            LocationKey = locationKey;
            Name = name;
            LocationPoint = point;
            DateCreated = created;
            LastUpdated = updated;
            Description = description;
        }

        public void SetLocation(string point)
        {
            throw new NotImplementedException();
        }
    }
}
