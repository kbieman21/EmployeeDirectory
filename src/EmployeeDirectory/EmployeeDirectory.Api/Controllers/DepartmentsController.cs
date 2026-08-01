using EmployeeDirectory.Application.DTOs;
using EmployeeDirectory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDirectory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();

            return Ok(departments);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DepartmentDto>> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);

            if (department is null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> Create(
            CreateDepartmentDto departmentDto)
        {
            var createdDepartment =
                await _departmentService.CreateAsync(departmentDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdDepartment.Id },
                createdDepartment);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateDepartmentDto departmentDto)
        {
            var updated =
                await _departmentService.UpdateAsync(id, departmentDto);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _departmentService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
