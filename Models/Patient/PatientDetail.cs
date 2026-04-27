namespace mPath.Models.Patient;

public class PatientDetail
{
    public PatientDetail()
    {
    }
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; } = new DateTime();
    public string Email { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
}