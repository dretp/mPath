using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using System.Security.Claims;
using System.Text;
using mPath.Interface;
using mPath.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using mPath.Utils.Auth;

namespace mPath.Services;

public class AuthService : IAuthService
{
    private readonly AuthUtil _authUtil;
    private readonly IConfiguration _configuration;

    public AuthService(AuthUtil authUtil, IConfiguration configuration)
    {
        _authUtil = authUtil;
        _configuration = configuration;
    }

    public async Task<EmployeeLoginResponse> EmployeeLogin(EmployeeLoginRequest loginRequest)
    {
        if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.PinCode))
        {
            return new EmployeeLoginResponse
            {
                IsAuthenticated = false,
                Message = "Pin code is required"
            };
        }

        if (!Regex.IsMatch(loginRequest.PinCode, "^\\d{6}$"))
        {
            return new EmployeeLoginResponse
            {
                IsAuthenticated = false,
                Message = "Pin code must be 6 digits"
            };
        }

        var loginResult = await _authUtil.EmployeeLogin(loginRequest.PinCode);
        if (!loginResult.IsAuthenticated)
        {
            return loginResult;
        }

        var expiresAt = DateTime.UtcNow.AddHours(8);
        loginResult.Token = generateJwtToken(loginResult, expiresAt);
        loginResult.ExpiresAtUtc = expiresAt;

        return loginResult;
    }

    private string generateJwtToken(EmployeeLoginResponse loginResult, DateTime expiresAt)
    {
        var issuer = _configuration["Jwt:Issuer"] ?? "mPath";
        var audience = _configuration["Jwt:Audience"] ?? "mPath.Client";
        var keyValue = _configuration["Jwt:Key"] ?? "mPath_dev_secret_key_please_change_2026";

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(keyValue);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, loginResult.EmployeeId.ToString()),
            new(JwtRegisteredClaimNames.Email, loginResult.Email),
            new(JwtRegisteredClaimNames.GivenName, loginResult.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, loginResult.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("employee_id", loginResult.EmployeeId.ToString())
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);
    }
}
