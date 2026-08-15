using EmployeeDirectory.Application.DTOs;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<AppUser> _passwordHasher;

        public AuthService(IUserRepository userRepository, PasswordHasher<AppUser> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> ValidateCredentialsAsync(LoginRequestDto loginRequest)
        {
            ArgumentNullException.ThrowIfNull(loginRequest);

            var user = await _userRepository.GetByEmailAsync(
                loginRequest.Email.Trim());

            if (user is null)
            {
                return false;
            }

            var result = _passwordHasher.VerifyHashedPassword(
                null!,
                user.PasswordHash,
                loginRequest.Password);

            return result != PasswordVerificationResult.Failed;
        }
    }
    }

