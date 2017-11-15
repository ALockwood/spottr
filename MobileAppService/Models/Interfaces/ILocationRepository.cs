using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using spottr.PostModels;

namespace spottr.Models
{
    public interface ILocationRepository
    {
        Task<Int32> Add(PostedLocation location);
        void Update(ILocation location);
        bool Remove(Int32 locationKey);
        Task<ILocation> GetAsync(Int32 locationKey);
        IEnumerable<ILocation> GetClosestLocations(double distanceKms, Int16 maxLocations);
        IEnumerable<ILocation> GetLatestLocations(Int16 maxLocations);
    }
}
