using System.Globalization;
using CsvHelper;
using mPath.Interface;
using mPath.Location.Models;
using mPath.Utils.Scheduling;

namespace mPath.Services;

public class SchedulingService : ISchedulingService
{
    private readonly SchedulingUtil _schedulingUtil;

    public SchedulingService(SchedulingUtil schedulingUtil)
    {
        _schedulingUtil = schedulingUtil;
    }

    public async Task<List<ShiftModel>> GetEmployeeSchedule(int employeeId, DateTime? dateUtc)
    {
        return await _schedulingUtil.GetEmployeeSchedule(employeeId, dateUtc);
    }

    public async Task<List<ShiftModel>> GetLocationSchedule(string storeId, DateTime? dateUtc)
    {
        return await _schedulingUtil.GetLocationSchedule(storeId, dateUtc);
    }

    public async Task<List<ShiftModel>> GetWeeklySchedule(string storeId, DateTime weekStartUtc)
    {
        return await _schedulingUtil.GetWeeklySchedule(storeId, weekStartUtc);
    }

    public async Task<ShiftModel?> GetShift(int shiftId)
    {
        return await _schedulingUtil.GetShiftById(shiftId);
    }

    public async Task<int> CreateShift(ShiftModel shift)
    {
        return await _schedulingUtil.CreateShift(shift);
    }

    public async Task<bool> UpdateShiftNotes(int shiftId, string notes)
    {
        return await _schedulingUtil.UpdateShiftNotes(shiftId, notes);
    }

    public async Task<bool> DeleteShift(int shiftId)
    {
        return await _schedulingUtil.DeleteShift(shiftId);
    }

    public async Task<List<AvailabilitySlotModel>> GetEmployeeAvailability(int employeeId)
    {
        return await _schedulingUtil.GetEmployeeAvailability(employeeId);
    }

    public async Task<ScheduleUploadResultModel> UploadScheduleCsv(Stream csvStream)
    {
        var result = new ScheduleUploadResultModel();

        using var streamReader = new StreamReader(csvStream);
        using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);
        csv.Context.Configuration.PrepareHeaderForMatch = args => args.Header.Trim().ToLowerInvariant();

        if (!await csv.ReadAsync())
        {
            return result;
        }

        csv.ReadHeader();

        while (await csv.ReadAsync())
        {
            result.TotalRows++;
            var rowNumber = csv.Context.Parser.Row;

            try
            {
                var employeeIdRaw = csv.GetField("employeeId");
                var locationIdRaw = csv.GetField("locationId");
                var roleIdRaw = csv.GetField("roleId");
                var startTimeRaw = csv.GetField("startTime");
                var endTimeRaw = csv.GetField("endTime");
                csv.TryGetField("notes", out string? notesRaw);

                if (!int.TryParse(employeeIdRaw, out var employeeId)
                    || !int.TryParse(locationIdRaw, out var locationId)
                    || !int.TryParse(roleIdRaw, out var roleId))
                {
                    result.Errors.Add(new ScheduleUploadErrorModel
                    {
                        RowNumber = rowNumber,
                        Message = "employeeId, locationId, and roleId must be valid integers"
                    });
                    continue;
                }

                if (!DateTime.TryParse(startTimeRaw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var startTime)
                    || !DateTime.TryParse(endTimeRaw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var endTime))
                {
                    result.Errors.Add(new ScheduleUploadErrorModel
                    {
                        RowNumber = rowNumber,
                        Message = "startTime and endTime must be valid ISO datetime values"
                    });
                    continue;
                }

                if (endTime <= startTime)
                {
                    result.Errors.Add(new ScheduleUploadErrorModel
                    {
                        RowNumber = rowNumber,
                        Message = "endTime must be after startTime"
                    });
                    continue;
                }

                var shiftId = await _schedulingUtil.CreateShift(new ShiftModel
                {
                    EmployeeId = employeeId,
                    LocationId = locationId,
                    RoleId = roleId,
                    StartTime = startTime,
                    EndTime = endTime,
                    Notes = notesRaw ?? string.Empty
                });

                if (shiftId <= 0)
                {
                    result.Errors.Add(new ScheduleUploadErrorModel
                    {
                        RowNumber = rowNumber,
                        Message = "failed to insert shift"
                    });
                    continue;
                }

                result.CreatedCount++;
            }
            catch (Exception ex)
            {
                result.Errors.Add(new ScheduleUploadErrorModel
                {
                    RowNumber = rowNumber,
                    Message = ex.Message
                });
            }
        }

        return result;
    }
}
