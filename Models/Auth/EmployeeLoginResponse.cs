using mPath.Models.Role;

namespace mPath.Models.Auth;

public class EmployeeLoginResponse
{
    public bool IsAuthenticated { get; set; }
    public int EmployeeId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime? ExpiresAtUtc { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<RoleModel> Roles { get; set; } = new();
}
