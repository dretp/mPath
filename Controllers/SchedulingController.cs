using Microsoft.AspNetCore.Mvc;
using mPath.Interface;
using mPath.Location.Models;

namespace mPath.Controllers;

[ApiController]
[Route("[Controller]")]
public class SchedulingController(ILogger<SchedulingController> logger, ISchedulingService svc) : Controller
{
    [HttpGet("employee/{employeeId:int}")]
    public async Task<IActionResult> GetEmployeeSchedule(int employeeId, [FromQuery] DateTime? dateUtc)
    {
        var shifts = await svc.GetEmployeeSchedule(employeeId, dateUtc);
        return Ok(shifts);
    }

    [HttpGet("location/{storeId}")]
    public async Task<IActionResult> GetLocationSchedule(string storeId, [FromQuery] DateTime? dateUtc)
    {
        var shifts = await svc.GetLocationSchedule(storeId, dateUtc);
        return Ok(shifts);
    }

    [HttpGet("weekly/{storeId}")]
    public async Task<IActionResult> GetWeeklySchedule(string storeId, [FromQuery] DateTime? weekStart)
    {
        var start = weekStart ?? DateTime.UtcNow;
        var shifts = await svc.GetWeeklySchedule(storeId, start);
        return Ok(shifts);
    }

    [HttpGet("shift/{shiftId:int}")]
    public async Task<IActionResult> GetShift(int shiftId)
    {
        var shift = await svc.GetShift(shiftId);
        if (shift == null)
        {
            return NotFound();
        }

        return Ok(shift);
    }

    [HttpGet("availability/{employeeId:int}")]
    public async Task<IActionResult> GetEmployeeAvailability(int employeeId)
    {
        var availability = await svc.GetEmployeeAvailability(employeeId);
        return Ok(availability);
    }

    [HttpPost("shift")]
    public async Task<IActionResult> CreateShift([FromBody] ShiftModel shift)
    {
        var shiftId = await svc.CreateShift(shift);
        if (shiftId <= 0)
        {
            return BadRequest();
        }

        return Ok(new { shiftId });
    }

    [HttpPatch("shift/{shiftId:int}/notes")]
    public async Task<IActionResult> UpdateShiftNotes(int shiftId, [FromBody] string notes)
    {
        var updated = await svc.UpdateShiftNotes(shiftId, notes);
        if (!updated)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("shift/{shiftId:int}")]
    public async Task<IActionResult> DeleteShift(int shiftId)
    {
        var deleted = await svc.DeleteShift(shiftId);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpPost("upload-csv")]
    public async Task<IActionResult> UploadScheduleCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "CSV file is required" });
        }

        await using var stream = file.OpenReadStream();
        var result = await svc.UploadScheduleCsv(stream);

        return Ok(result);
    }
}
