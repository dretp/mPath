using Microsoft.AspNetCore.Mvc;
using mPath.Interface;
using mPath.Models.Auth;

namespace mPath.Controllers;

[ApiController]
[Route("[Controller]")]
public class AuthController(ILogger<AuthController> logger, IAuthService svc) : Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] EmployeeLoginRequest request)
    {
        var result = await svc.EmployeeLogin(request);

        if (!result.IsAuthenticated)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }
}
