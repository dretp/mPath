using Microsoft.AspNetCore.Mvc;
using mPath.Interface;

namespace mPath.Controllers;

[ApiController]
[Route("[Controller]")]
public class LocationController(ILogger<LocationController> logger, ILocationService svc) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetLocations()
    {
        var locations = await svc.GetLocations();
        return Ok(locations);
    }

    [HttpGet("{storeId}")]
    public async Task<IActionResult> GetLocation(string storeId)
    {
        var location = await svc.GetLocation(storeId);
        if (location == null)
        {
            return NotFound();
        }

        return Ok(location);
    }
}
