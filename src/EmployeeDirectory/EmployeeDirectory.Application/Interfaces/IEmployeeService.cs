using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDirectory.Application.DTOs;

namespace EmployeeDirectory.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();

        Task<EmployeeDto?> GetByIdAsync(int id);

        Task<EmployeeDto> CreateAsync(CreateEmployeeDto employeeDto);

        Task<bool> UpdateAsync(int id, UpdateEmployeeDto employeeDto);

        Task<bool> DeleteAsync(int id);
    }
}
