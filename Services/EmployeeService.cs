using mPath.Interface;
using mPath.Models;
using mPath.Models.Employees;

using mPath.Utils;
using mPath.Utils.EmployeeUitls;

namespace mPath.Services;

public class EmployeeService:IEmployeeService
{
    private readonly EmployeeUtils _employeeUtils;

    public EmployeeService(EmployeeUtils employeeUtils)
    {
            _employeeUtils = employeeUtils; 
    }


    public async Task<List<EmployeeModel>> GetEmployees()
    {
        return await _employeeUtils.GetEmployees();
    }

    
}