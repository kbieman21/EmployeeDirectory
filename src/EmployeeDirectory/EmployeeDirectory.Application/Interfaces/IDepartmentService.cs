using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDirectory.Application.DTOs;

namespace EmployeeDirectory.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync();

        Task<DepartmentDto?> GetByIdAsync(int id);

        Task<DepartmentDto> CreateAsync(CreateDepartmentDto departmentDto);

        Task<bool> UpdateAsync(int id, UpdateDepartmentDto departmentDto);

        Task<bool> DeleteAsync(int id);
    }
}
