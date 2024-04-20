namespace ASPNETCore8ApiIOracleEf.Services
{
    using ASPNETCore8ApiIOracleEf.Models;
    using ASPNETCore8ApiIOracleEf.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class EmployeesService : IEmployeesService
    {
        private readonly HrDbContext _context;

        public EmployeesService(HrDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeViewModel>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Select(emp => new EmployeeViewModel
                {
                    Id = emp.Id,
                    Name = emp.Name,
                    Address = emp.Address,
                    Photo = emp.Photo
                })
                .ToListAsync();
        }

        public async Task<EmployeeViewModel?> GetEmployeeByIdAsync(decimal id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return null;
            return new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Address = employee.Address,
                Photo = employee.Photo
            };
        }

        public async Task<EmployeeViewModel?> AddEmployeeAsync(EmployeeViewModel model)
        {
            var employee = new Employee
            {
                Name = model.Name,
                Address = model.Address,
                Photo = model.Photo
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            model.Id = employee.Id;
            return model;
        }

        public async Task<bool> UpdateEmployeeAsync(decimal id, EmployeeViewModel model)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            employee.Name = model.Name;
            employee.Address = model.Address;
            employee.Photo = model.Photo;

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(decimal id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
