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
    /// <remarks>
    /// [Produces("application/json")]: 在 swagger 會提示回傳 json 的內容
    /// </remarks>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class EmployeesNonRestController : ControllerBase
    {
        private readonly IEmployeesService _service;

        /// <summary>
        /// 員工基本資料維護 建構子 <see cref="EmployeesNonRestController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public EmployeesNonRestController(IEmployeesService service)
        {
            _service = service;
        }

        /// <summary>
        /// 查詢全部員工
        /// </summary>
        /// <returns></returns>
        // GET: api/Employees
        [HttpGet(Name = nameof(GetEmployeesAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmployeeViewModel>>> GetEmployeesAsync()
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
        [HttpGet("{id}", Name = nameof(GetEmployeeByIdAsync))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeViewModel>> GetEmployeeByIdAsync(decimal id)
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
        [HttpPost(Name = nameof(AddEmployeeAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EmployeeViewModel>> AddEmployeeAsync(EmployeeViewModel employee)
        {
            var createdEmployee = await _service.AddEmployeeAsync(employee);
            if (createdEmployee == null)
            {
                // Handle the case when employee is not created, perhaps return an error response
                return Conflict("Conflict: Unable to create employee");
            }

            //// 這個會出錯: no route matches the supplied values 
            //// 解決方式: 參考保哥的文章
            //// https://blog.miniasp.com/post/2023/04/19/ASPNET-Core-Web-API-CreatedAtAction-No-route-matches-the-supplied-values
            //// --> ASP.NET Core Web API 遭遇 No route matches the supplied values 的問題
            //// --> 將 CreatedAtAction(...) 改為 CreatedAtRoute(...)
            //return CreatedAtAction(nameof(GetEmployeeByIdAsync), new { id = createdEmployee.Id }, createdEmployee);
            return CreatedAtRoute(nameof(GetEmployeeByIdAsync), new { id = createdEmployee.Id }, createdEmployee);
        }

        /// <summary>
        /// 修改員工
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        // PUT: api/Employees/5
        [HttpPost("{id}", Name = nameof(UpdateEmployeeAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEmployeeAsync(decimal id, EmployeeViewModel employee)
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
        [HttpPost("{id}", Name = nameof(DeleteEmployeeAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmployeeAsync(decimal id)
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
