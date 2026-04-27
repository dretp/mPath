using Microsoft.AspNetCore.Mvc;
using mPath.Interface;

namespace mPath.Controllers;

[ApiController]
[Route("[Controller]")]
public class RoleController(ILogger<RoleController> logger, IRoleService svc) : Controller
{
    private readonly ILogger<RoleController> _logger = logger;
    private readonly IRoleService _svc = svc;

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _svc.GetRoles();
        return Ok(roles);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRole(int id)
    {
        var role = await _svc.GetRole(id);
        if (role == null) return NotFound();
        return Ok(role);
    }
}
