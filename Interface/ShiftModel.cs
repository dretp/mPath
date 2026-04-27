namespace mPath.Location.Models;

public class ShiftModel
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public int RoleId { get; set; }
    public string StoreId { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; } = new DateTime();
    public DateTime EndTime { get; set; } = new DateTime();
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = new DateTime();
}
