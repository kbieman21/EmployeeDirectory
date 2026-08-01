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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly EmployeeDirectoryDbContext _context;

        public DepartmentRepository(EmployeeDirectoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Department> AddAsync(Department department)
        {
            _context.Departments.Add(department);

            await _context.SaveChangesAsync();

            return department;
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var department = await GetByIdAsync(id);

            if (department == null)
                return;

            _context.Departments.Remove(department);

            await _context.SaveChangesAsync();
        }


    }
}
