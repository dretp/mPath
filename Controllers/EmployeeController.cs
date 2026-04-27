using Microsoft.AspNetCore.Mvc;
using mPath.Interface;

namespace mPath.Controllers;

[ApiController]
[Route("[Controller]")]
public class EmployeeController(ILogger<EmployeeController> logger, IEmployeeService svc) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await svc.GetEmployees();
        return Ok(employees);
    }
}