using EmployeeDirectory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByEmailAsync(string email);

        Task<AppUser> AddAsync(AppUser user);
    }
}
