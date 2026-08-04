using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using EmployeeDirectory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDirectory.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDirectoryDbContext _context;

        public EmployeeRepository(EmployeeDirectoryDbContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<Employee>> GetAllAsync()
        //{
        //    return await _context.Employees
        //        .AsNoTracking()
        //        .Include(e => e.Department)
        //        .OrderBy(e => e.LastName)
        //        .ThenBy(e => e.FirstName)
        //        .ToListAsync();
        //}

        public async Task<(
    IReadOnlyCollection<Employee> Items,
    int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        int? departmentId,
        string sortBy,
        bool sortDescending)
        {
            var query = _context.Employees
                .AsNoTracking()
                .Include(employee => employee.Department)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var normalizedSearch = search.Trim();

                query = query.Where(employee =>
                    employee.FirstName.Contains(normalizedSearch) ||
                    employee.LastName.Contains(normalizedSearch) ||
                    employee.Email.Contains(normalizedSearch) ||
                    employee.JobTitle.Contains(normalizedSearch));
            }

            if (departmentId.HasValue)
            {
                query = query.Where(employee =>
                    employee.DepartmentId == departmentId.Value);
            }

            var totalCount = await query.CountAsync();

            query = ApplySorting(
                query,
                sortBy,
                sortDescending);

            var skip = (pageNumber - 1) * pageSize;

            var employees = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return (employees, totalCount);
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .FirstAsync(e => e.Id == employee.Id);
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(
            string email,
            int? excludedEmployeeId = null)
        {
            var normalizedEmail = email.Trim().ToLower();

            return await _context.Employees.AnyAsync(e =>
                e.Email.ToLower() == normalizedEmail &&
                (!excludedEmployeeId.HasValue ||
                 e.Id != excludedEmployeeId.Value));
        }

        private static IQueryable<Employee> ApplySorting(
    IQueryable<Employee> query,
    string sortBy,
    bool descending)
        {
            var normalizedSortBy =
                sortBy.Trim().ToLowerInvariant();

            return normalizedSortBy switch
            {
                "firstname" => descending
                    ? query.OrderByDescending(employee => employee.FirstName)
                        .ThenByDescending(employee => employee.Id)
                    : query.OrderBy(employee => employee.FirstName)
                        .ThenBy(employee => employee.Id),

                "email" => descending
                    ? query.OrderByDescending(employee => employee.Email)
                        .ThenByDescending(employee => employee.Id)
                    : query.OrderBy(employee => employee.Email)
                        .ThenBy(employee => employee.Id),

                "jobtitle" => descending
                    ? query.OrderByDescending(employee => employee.JobTitle)
                        .ThenByDescending(employee => employee.Id)
                    : query.OrderBy(employee => employee.JobTitle)
                        .ThenBy(employee => employee.Id),

                "hiredate" => descending
                    ? query.OrderByDescending(employee => employee.HireDate)
                        .ThenByDescending(employee => employee.Id)
                    : query.OrderBy(employee => employee.HireDate)
                        .ThenBy(employee => employee.Id),

                "department" => descending
                    ? query.OrderByDescending(employee => employee.Department.Name)
                        .ThenByDescending(employee => employee.Id)
                    : query.OrderBy(employee => employee.Department.Name)
                        .ThenBy(employee => employee.Id),

                _ => descending
                    ? query.OrderByDescending(employee => employee.LastName)
                        .ThenByDescending(employee => employee.FirstName)
                        .ThenByDescending(employee => employee.Id)
                    : query.OrderBy(employee => employee.LastName)
                        .ThenBy(employee => employee.FirstName)
                        .ThenBy(employee => employee.Id)
            };
        }
    }
}
