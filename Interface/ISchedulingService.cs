using mPath.Location.Models;

namespace mPath.Interface;

public interface ISchedulingService
{
    Task<List<ShiftModel>> GetEmployeeSchedule(int employeeId, DateTime? dateUtc);
    Task<List<ShiftModel>> GetLocationSchedule(string storeId, DateTime? dateUtc);
    Task<List<ShiftModel>> GetWeeklySchedule(string storeId, DateTime weekStartUtc);
    Task<ShiftModel?> GetShift(int shiftId);
    Task<int> CreateShift(ShiftModel shift);
    Task<bool> UpdateShiftNotes(int shiftId, string notes);
    Task<bool> DeleteShift(int shiftId);
    Task<List<AvailabilitySlotModel>> GetEmployeeAvailability(int employeeId);
    Task<ScheduleUploadResultModel> UploadScheduleCsv(Stream csvStream);
}
