using System.Data;
using System.Text;
using mPath.Models.Employees;
using mPath.Utils.Base;

namespace mPath.Utils.EmployeeUitls;

public class EmployeeUtils:BaseUtils
{

    public EmployeeUtils()
    {
        
    }
    
    public async Task<List<EmployeeModel>> GetEmployees()
    {
        return await allEmployees();
    }


    private async Task<List<EmployeeModel>> allEmployees()
    {
        var employeeList = new List<EmployeeModel>();

        var sql = new StringBuilder();

        sql.AppendLine(
            "SELECT employees.first_name, employees.last_name,employees.email,employees.pin_code FROM employees;");

        try
        {
            await using var dataSource = base.dataSource();
            
            await using var cmd = command(dataSource, sql.ToString());
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var employeeData = new EmployeeModel
                {
                    firstName = reader["first_name"].ToString(),
                    lastName = reader["last_name"].ToString(),
                    email = reader["email"].ToString()
                };
                employeeList.Add(employeeData);
            }
            
            return employeeList;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        

    }


}
