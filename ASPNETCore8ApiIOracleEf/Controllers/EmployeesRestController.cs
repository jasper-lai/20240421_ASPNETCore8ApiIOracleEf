using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETCore8ApiIOracleEf.Models;
using ASPNETCore8ApiIOracleEf.Services;
using ASPNETCore8ApiIOracleEf.ViewModels;

namespace ASPNETCore8ApiIOracleEf.Controllers
{

    /// <summary>
    /// 員工基本資料維護作業
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EmployeesRestController : ControllerBase
    {
        private readonly IEmployeesService _service;

        /// <summary>
        /// 員工基本資料維護 建構子 <see cref="EmployeesRestController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public EmployeesRestController(IEmployeesService service)
        {
            _service = service;
        }

        /// <summary>
        /// 查詢全部員工
        /// </summary>
        /// <returns></returns>
        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeViewModel>>> GetEmployees()
        {
            var employees = await _service.GetAllEmployeesAsync();
            return Ok(employees);
        }

        /// <summary>
        /// 查詢單一員工 (by 員工編號)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeViewModel>> GetEmployee(decimal id)
        {
            var employee = await _service.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        /// <summary>
        /// 新增員工
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<EmployeeViewModel>> PostEmployee(EmployeeViewModel employee)
        {
            var createdEmployee = await _service.AddEmployeeAsync(employee);
            if (createdEmployee == null)
            {
                // Handle the case when employee is not created, perhaps return an error response
                return BadRequest("Unable to create employee");
            }
            return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.Id }, createdEmployee);
        }

        /// <summary>
        /// 修改員工
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(decimal id, EmployeeViewModel employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            var result = await _service.UpdateEmployeeAsync(id, employee);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// 刪除員工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(decimal id)
        {
            var result = await _service.DeleteEmployeeAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
