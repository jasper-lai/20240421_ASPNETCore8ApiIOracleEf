using ASPNETCore8ApiIOracleEf.ViewModels;

namespace ASPNETCore8ApiIOracleEf.Services;

public interface IEmployeesService
{
    Task<List<EmployeeViewModel>> GetAllEmployeesAsync();
    Task<EmployeeViewModel?> GetEmployeeByIdAsync(decimal id);
    Task<EmployeeViewModel?> AddEmployeeAsync(EmployeeViewModel model);
    Task<bool> UpdateEmployeeAsync(decimal id, EmployeeViewModel model);
    Task<bool> DeleteEmployeeAsync(decimal id);
}