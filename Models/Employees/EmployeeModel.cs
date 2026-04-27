namespace mPath.Models.Employees;

public class EmployeeModel
{
    public EmployeeModel()
    {
    }
    
    public String firstName { get; set; }
    public String lastName { get; set; }
    public String email { get; set; }
    public String phoneNumber { get; set; }
    public int pin { get; set; }
    public DateTime hireDate { get; set; }
    public bool isActive { get; set; }
    
    
}