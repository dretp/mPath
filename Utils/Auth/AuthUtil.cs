using System.Text;
using mPath.Models.Auth;
using mPath.Models.Role;
using mPath.Utils.Base;

namespace mPath.Utils.Auth;

public class AuthUtil : BaseUtils
{
    public AuthUtil()
    {
    }

    public async Task<EmployeeLoginResponse> EmployeeLogin(string pinCode)
    {
        return await retrieveEmployeeByPin(pinCode);
    }

    private async Task<EmployeeLoginResponse> retrieveEmployeeByPin(string pinCode)
    {
        var response = new EmployeeLoginResponse
        {
            IsAuthenticated = false,
            Message = "Invalid login"
        };

        var sql = new StringBuilder();
        sql.AppendLine("SELECT id, first_name, last_name, email, is_active");
        sql.AppendLine("FROM employees");
        sql.AppendLine("WHERE pin_code = @pin_code");
        sql.AppendLine("LIMIT 1;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            cmd.Parameters.AddWithValue("@pin_code", pinCode);
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var isActive = reader.GetBoolean(reader.GetOrdinal("is_active"));
                if (!isActive)
                {
                    response.Message = "Employee is inactive";
                    return response;
                }

                response.IsAuthenticated = true;
                response.EmployeeId = reader.GetInt32(reader.GetOrdinal("id"));
                response.FirstName = reader["first_name"].ToString() ?? string.Empty;
                response.LastName = reader["last_name"].ToString() ?? string.Empty;
                response.Email = reader["email"].ToString() ?? string.Empty;
                response.Message = "Login successful";
                response.Roles = await retrieveEmployeeRoles(response.EmployeeId);
            }

            return response;
        }
        catch (Exception e)
        {
            LogError(e, "AuthUtil.retrieveEmployeeByPin");
            response.Message = "Login error";
            return response;
        }
    }

    private async Task<List<RoleModel>> retrieveEmployeeRoles(int employeeId)
    {
        var roles = new List<RoleModel>();

        var sql = new StringBuilder();
        sql.AppendLine("SELECT r.id, r.name");
        sql.AppendLine("FROM roles r");
        sql.AppendLine("JOIN employee_roles er ON er.role_id = r.id");
        sql.AppendLine("WHERE er.employee_id = @employee_id;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            cmd.Parameters.AddWithValue("@employee_id", employeeId);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                roles.Add(new RoleModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader["name"].ToString() ?? string.Empty
                });
            }

            return roles;
        }
        catch (Exception e)
        {
            LogError(e, "AuthUtil.retrieveEmployeeRoles");
            return roles;
        }
    }
}
