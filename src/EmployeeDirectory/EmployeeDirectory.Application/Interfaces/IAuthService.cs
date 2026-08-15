using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDirectory.Application.DTOs;

namespace EmployeeDirectory.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ValidateCredentialsAsync(LoginRequestDto loginRequest);
    }
}
