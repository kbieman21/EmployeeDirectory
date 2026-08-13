using EmployeeDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDirectory.Infrastructure.Data
{
    public class EmployeeDirectoryDbContext : DbContext
    {
        public EmployeeDirectoryDbContext(DbContextOptions<EmployeeDirectoryDbContext> options)
        : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<AppUser> Users => Set<AppUser>();

    }
}
