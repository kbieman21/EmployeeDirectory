using EmployeeDirectory.Application.DTOs;
using EmployeeDirectory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDirectory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        //{
        //    var employees = await _employeeService.GetAllAsync();

        //    return Ok(employees);
        //}

        [HttpGet]
        public async Task<ActionResult<PagedResult<EmployeeDto>>> GetPaged(
    [FromQuery] EmployeeQueryParameters parameters)
        {
            var result = await _employeeService.GetPagedAsync(parameters);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeDto>> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee is null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        //[HttpPost]
        //public async Task<ActionResult<EmployeeDto>> Create(
        //    CreateEmployeeDto employeeDto)
        //{
        //    try
        //    {
        //        var createdEmployee =
        //            await _employeeService.CreateAsync(employeeDto);

        //        return CreatedAtAction(
        //            nameof(GetById),
        //            new { id = createdEmployee.Id },
        //            createdEmployee);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return Conflict(new { message = ex.Message });
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Create(
    CreateEmployeeDto employeeDto)
        {
            var createdEmployee =
                await _employeeService.CreateAsync(employeeDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdEmployee.Id },
                createdEmployee);
        }

        //[HttpPut("{id:int}")]
        //public async Task<IActionResult> Update(
        //    int id,
        //    UpdateEmployeeDto employeeDto)
        //{
        //    try
        //    {
        //        var updated =
        //            await _employeeService.UpdateAsync(id, employeeDto);

        //        if (!updated)
        //        {
        //            return NotFound();
        //        }

        //        return NoContent();
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return Conflict(new { message = ex.Message });
        //    }
        //}

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
    int id,
    UpdateEmployeeDto employeeDto)
        {
            var updated =
                await _employeeService.UpdateAsync(id, employeeDto);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _employeeService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
