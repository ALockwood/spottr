using System;

namespace spottr.Models
{
    public interface ILocation
    {
        Int32 LocationKey { get; }
        string Name { get; set; }
        string Description { get; set; }
        double Latitude { get; }
        double Longitude { get; }
        DateTime DateCreated { get; }
        DateTime LastUpdated { get; }

        void SetLocationLatitudeAndLongitude(string point); 
    }
}