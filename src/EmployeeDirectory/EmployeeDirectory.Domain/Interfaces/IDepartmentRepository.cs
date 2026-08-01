using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Domain.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();

        Task<Department?> GetByIdAsync(int id);

        Task<Department> AddAsync(Department department);

        Task UpdateAsync(Department department);

        Task DeleteAsync(int id);
    }
}
