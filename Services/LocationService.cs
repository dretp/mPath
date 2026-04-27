using mPath.Interface;
using mPath.Location.Models;
using mPath.Utils.Location;

namespace mPath.Services;

public class LocationService : ILocationService
{
    private readonly LocationUtil _locationUtil;

    public LocationService(LocationUtil locationUtil)
    {
        _locationUtil = locationUtil;
    }

    public async Task<LocationModel?> GetLocation(string storeId)
    {
        return await _locationUtil.GetLocationByStoreId(storeId);
    }

    public async Task<List<LocationModel>> GetLocations()
    {
        return await _locationUtil.GetLocations();
    }

    public async Task<bool> CreateLocation(LocationModel location)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateLocation(LocationModel location)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteLocation(string storeId)
    {
        throw new NotImplementedException();
    }
}
