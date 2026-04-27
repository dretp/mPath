using Microsoft.AspNetCore.Mvc;
using mPath.Interface;

namespace mPath.Controllers;

[ApiController]
[Route("[Controller]")]
public class PatientController (ILogger<PatientController> logger, IPatientService svc)  : Controller
{
    // GET
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPatient(int id)
    {
        var patient = await svc.GetPatient(id);
        return Ok(patient);
    }
}