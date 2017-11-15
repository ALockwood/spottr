using System;
using Npgsql;
using spottr.Models;
using System.Threading.Tasks;
using spottr.PostModels;
using NpgsqlTypes;
using System.Collections.Generic;

namespace spottr.DAL
{
    public class PostgresLocationDAL : ILocationDAL
    {
        readonly string _connectionString;

        public PostgresLocationDAL(string hostName, string databaseName, string userName, string password, Int16 portNumber = 5432)
        {
            if (string.IsNullOrWhiteSpace(hostName) || string.IsNullOrWhiteSpace(databaseName) || userName == null || password == null)
            {
                throw new ArgumentException($"One or more argument passed was null or empty. Check values for {nameof(hostName)}, {nameof(databaseName)}, {nameof(userName)}, {nameof(password)}");
            }

            try
            {
                var bldr = new NpgsqlConnectionStringBuilder
                {
                    Host = hostName,
                    Database = databaseName,
                    Username = userName,
                    Password = password,
                    Port = portNumber
                };
                using (var c = new NpgsqlConnection(bldr.ConnectionString))
                {
                    c.Open();
                    var cmd = new NpgsqlCommand("SELECT 1;", c).ExecuteNonQuery();
                    c.Close();
                }

                _connectionString = bldr.ConnectionString;

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException ($"Failed to validate database connection! Ex: {ex.Message}");
            }
        }

        public async Task<ILocation> GetLocation(int locationKey)
        {
            PostGisLocation location = null;

            using (var c = new NpgsqlConnection(_connectionString))
            {
                c.Open();

                var cmd = new NpgsqlCommand
                {
                    Connection = c,
                    CommandType = System.Data.CommandType.Text,
                    CommandText = $"SELECT name, date_added, COALESCE(date_updated, date_added) AS date_updated, description, ST_AsText(location_data) AS location_data FROM public.locations WHERE location_key = @locationKey;"
                };
                cmd.Parameters.Add("locationKey", NpgsqlDbType.Integer);
                cmd.Parameters[0].Value = locationKey;
                cmd.Prepare();

                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    location = new PostGisLocation(locationKey, reader.GetString(0), reader.GetString(4), reader.GetDateTime(1), reader.GetDateTime(2), reader.GetString(3));
                }

                c.Close();
            }

            return location;
        }

        public async Task<Int32> AddLocation(PostedLocation location)
        {
            Int32 retval = -1;

            using (var c = new NpgsqlConnection(_connectionString))
            {
                c.Open();

                var cmd = new NpgsqlCommand
                {
                    Connection = c,
                    CommandType = System.Data.CommandType.Text,
                    CommandText = "INSERT INTO public.locations(name, location_data, description) VALUES(@name, ST_SetSRID(ST_MakePoint(@latitude, @longitude), 4326), @description) RETURNING location_key;"
                };

                cmd.Parameters.AddWithValue("name", NpgsqlDbType.Varchar, location.Name);
                cmd.Parameters.AddWithValue("latitude", NpgsqlDbType.Double, location.Latitude);
                cmd.Parameters.AddWithValue("longitude", NpgsqlDbType.Double, location.Longitude);
                cmd.Parameters.AddWithValue("description", NpgsqlDbType.Text, location.Description);

                cmd.Prepare();

                retval = (Int32)await cmd.ExecuteScalarAsync();

                c.Close();
            }

            return retval;
        }

        public IEnumerable<ILocation> GetLatestLocations(Int16 maxLocations)
        {
            using (var c = new NpgsqlConnection(_connectionString))
            {
                c.Open();

                var cmd = new NpgsqlCommand
                {
                    Connection = c,
                    CommandType = System.Data.CommandType.Text,
                    CommandText = $"SELECT location_key, name, date_added, COALESCE(date_updated, date_added) AS date_updated, description, ST_AsText(location_data) AS location_data FROM public.locations ORDER BY date_updated DESC LIMIT @limit;"
                };
                cmd.Parameters.AddWithValue("limit", NpgsqlDbType.Integer, maxLocations);

                cmd.Prepare();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    yield return new PostGisLocation(reader.GetInt32(0), reader.GetString(1), reader.GetString(5), reader.GetDateTime(2), reader.GetDateTime(3), reader.GetString(4));
                }

                c.Close();
            }
        }
    }
}
