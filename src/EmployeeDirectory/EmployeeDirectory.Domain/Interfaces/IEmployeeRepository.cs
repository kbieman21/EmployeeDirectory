using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        //Task<IEnumerable<Employee>> GetAllAsync();
        Task<(IReadOnlyCollection<Employee> Items, int TotalCount)>
    GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        int? departmentId,
        string sortBy,
        bool sortDescending);

        Task<Employee?> GetByIdAsync(int id);

        Task<Employee> AddAsync(Employee employee);

        Task UpdateAsync(Employee employee);

        Task DeleteAsync(Employee employee);

        Task<bool> EmailExistsAsync(string email, int? excludedEmployeeId = null);
    }
}
