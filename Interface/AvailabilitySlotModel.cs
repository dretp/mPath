namespace mPath.Location.Models;

public class AvailabilitySlotModel
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; } = new TimeSpan();
    public TimeSpan EndTime { get; set; } = new TimeSpan();
}
