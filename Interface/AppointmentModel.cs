namespace mPath.Location.Models;

public enum AppointmentStatus
{
    Scheduled = 0,
    Confirmed = 1,
    Completed = 2,
    Cancelled = 3,
    NoShow = 4
}

public class AppointmentModel
{
    public int AppointmentId { get; set; }
    public string StoreId { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime StartTimeUtc { get; set; } = new DateTime();
    public DateTime EndTimeUtc { get; set; } = new DateTime();
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public string Notes { get; set; } = string.Empty;
}
