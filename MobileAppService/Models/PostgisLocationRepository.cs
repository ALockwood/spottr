using System;
using System.Collections.Generic;
using spottr.DAL;
using System.Threading.Tasks;
using spottr.PostModels;

namespace spottr.Models
{
    public class PostgisLocationRepository : ILocationRepository
    {
        readonly ILocationDAL _dal;

        public PostgisLocationRepository(ILocationDAL locationDAL)
        {
            _dal = locationDAL;
        }

        public async Task<Int32> Add(PostedLocation location)
        {
            return await _dal.AddLocation(location);
        }

        public async Task<ILocation> GetAsync(int locationKey)
        {
            return await _dal.GetLocation(locationKey);
        }

        public IEnumerable<ILocation> GetClosestLocations(double distanceKms, short maxLocations)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ILocation> GetLatestLocations(short maxLocations)
        {
            return _dal.GetLatestLocations(maxLocations);
        }

        public bool Remove(int locationKey)
        {
            throw new NotImplementedException();
        }

        public void Update(ILocation location)
        {
            throw new NotImplementedException();
        }
    }
}
