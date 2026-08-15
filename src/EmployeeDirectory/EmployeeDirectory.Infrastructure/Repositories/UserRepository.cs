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
    public class UserRepository : IUserRepository
    {
        private readonly EmployeeDirectoryDbContext _context;

        public UserRepository(EmployeeDirectoryDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<AppUser> AddAsync(AppUser user)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }
    }
}
