using System;

namespace spottr.Models
{
    public interface ILocation
    {
        Int32 LocationKey { get; }
        string Name { get; set; }
        string Description { get; set; }
        string LocationPoint { get; }
        DateTime DateCreated { get; }
        DateTime LastUpdated { get; }

        void SetLocation(string point);
        //TODO: Add SetLastUpdated?
    }
}