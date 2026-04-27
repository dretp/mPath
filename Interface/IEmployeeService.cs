using mPath.Models.Employees;

namespace mPath.Interface;

public interface IEmployeeService
{ 
  
  Task<List<EmployeeModel>> GetEmployees();
}