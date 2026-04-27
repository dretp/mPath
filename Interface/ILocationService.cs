using mPath.Location.Models;

namespace mPath.Interface;

public interface ILocationService
{
    Task<LocationModel?> GetLocation(string storeId);
    Task<List<LocationModel>> GetLocations();
    Task<bool> CreateLocation(LocationModel location);
    Task<bool> UpdateLocation(LocationModel location);
    Task<bool> DeleteLocation(string storeId);
}
