using System.Text;
using mPath.Location.Models;
using mPath.Utils.Base;

namespace mPath.Utils.Location;

public class LocationUtil : BaseUtils
{
    public LocationUtil()
    {
    }

    public async Task<List<LocationModel>> GetLocations()
    {
        return await retrieveLocations();
    }

    public async Task<LocationModel?> GetLocationByStoreId(string storeId)
    {
        return await retrieveLocationByStoreId(storeId);
    }

    private async Task<List<LocationModel>> retrieveLocations()
    {
        var locations = new List<LocationModel>();

        var sql = new StringBuilder();
        sql.AppendLine("SELECT id, store_id, name, address, city, state, zip_code FROM locations ORDER BY id;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var location = new LocationModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    StoreId = reader["store_id"].ToString() ?? string.Empty,
                    Name = reader["name"].ToString() ?? string.Empty,
                    Address = reader["address"].ToString() ?? string.Empty,
                    City = reader["city"].ToString() ?? string.Empty,
                    State = reader["state"].ToString() ?? string.Empty,
                    ZipCode = reader["zip_code"].ToString() ?? string.Empty
                };

                locations.Add(location);
            }

            return locations;
        }
        catch (Exception e)
        {
            LogError(e, "LocationUtil.retrieveLocations");
            return locations;
        }
    }

    private async Task<LocationModel?> retrieveLocationByStoreId(string storeId)
    {
        var sql = new StringBuilder();
        sql.AppendLine("SELECT id, store_id, name, address, city, state, zip_code FROM locations WHERE store_id = @store_id;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            cmd.Parameters.AddWithValue("@store_id", storeId);
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new LocationModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    StoreId = reader["store_id"].ToString() ?? string.Empty,
                    Name = reader["name"].ToString() ?? string.Empty,
                    Address = reader["address"].ToString() ?? string.Empty,
                    City = reader["city"].ToString() ?? string.Empty,
                    State = reader["state"].ToString() ?? string.Empty,
                    ZipCode = reader["zip_code"].ToString() ?? string.Empty
                };
            }

            return null;
        }
        catch (Exception e)
        {
            LogError(e, "LocationUtil.retrieveLocationByStoreId");
            return null;
        }
    }
}
