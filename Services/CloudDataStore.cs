using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Plugin.Connectivity;

namespace spottr
{
    public class CloudDataStore : IDataStore<Location>
    {
        HttpClient client;
        IEnumerable<Location> locations;

        public CloudDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.BackendUrl}/");

            locations = new List<Location>();
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/location");
                locations = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Location>>(json));
            }

            return locations;
        }

        public async Task<Location> GetLocationAsync(Int32 locationKey)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/location/{locationKey}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Location>(json));
            }

            return null;
        }

        public async Task<bool> AddLocationAsync(Location location)
        {
            if (location == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(location);

            var response = await client.PostAsync($"api/location", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateLocationAsync(Location location)
        {
            if (location == null || location.LocationKey == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(location);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/location/{location.LocationKey}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteLocationAsync(Int32 locationKey)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var response = await client.DeleteAsync($"api/location/{locationKey}");

            return response.IsSuccessStatusCode;
        }
    }
}
