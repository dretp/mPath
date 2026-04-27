using mPath.Models.Auth;

namespace mPath.Interface;

public interface IAuthService
{
    Task<EmployeeLoginResponse> EmployeeLogin(EmployeeLoginRequest loginRequest);
}
