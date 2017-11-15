using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace spottr
{
    public interface IDataStore<T>
    {
        Task<bool> AddLocationAsync(T item);
        Task<bool> UpdateLocationAsync(T item);
        Task<bool> DeleteLocationAsync(Int32 id);
        Task<T> GetLocationAsync(Int32 id);
        Task<IEnumerable<T>> GetLocationsAsync(bool forceRefresh = false);
    }
}
