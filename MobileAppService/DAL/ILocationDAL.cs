using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using spottr.Models;
using spottr.PostModels;

namespace spottr.DAL
{
    public interface ILocationDAL
    {
        Task<ILocation> GetLocation(Int32 locationKey);
        Task<Int32> AddLocation(PostedLocation location);
        IEnumerable<ILocation> GetLatestLocations(Int16 maxLocations);
    }
}
