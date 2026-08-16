using EmployeeDirectory.Application.Models;
using EmployeeDirectory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Interfaces
{
    public interface ITokenService
    {
        TokenResult CreateToken(AppUser user);
    }
}
