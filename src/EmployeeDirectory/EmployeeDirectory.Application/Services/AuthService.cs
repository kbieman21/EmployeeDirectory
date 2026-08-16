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
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<AppUser> _passwordHasher;

        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService,
            PasswordHasher<AppUser> passwordHasher)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        //public async Task<bool> ValidateCredentialsAsync(LoginRequestDto loginRequest)
        //{
        //    ArgumentNullException.ThrowIfNull(loginRequest);

        //    var user = await _userRepository.GetByEmailAsync(
        //        loginRequest.Email.Trim());

        //    if (user is null)
        //    {
        //        return false;
        //    }

        //    var result = _passwordHasher.VerifyHashedPassword(
        //        null!,
        //        user.PasswordHash,
        //        loginRequest.Password);

        //    return result != PasswordVerificationResult.Failed;
        //}
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            ArgumentNullException.ThrowIfNull(loginRequest);
            var user = await _userRepository.GetByEmailAsync(
                loginRequest.Email.Trim());
            if (user is null)
            {
                //throw new UnauthorizedAccessException("Invalid email or password.");
                return null;
            }
            // Verify the password
            //var passwordHasher = new PasswordHasher<AppUser>();
            var passwordResult =
        _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            loginRequest.Password);

            //var result = passwordHasher.VerifyHashedPassword(
            //    user,
            //    user.PasswordHash,
            //    loginRequest.Password);
            if (passwordResult == PasswordVerificationResult.Failed)
            {
                //throw new UnauthorizedAccessException("Invalid email or password.");
                return null;
            }
            // Generate a token
            var tokenResult = _tokenService.CreateToken(user);
            return new LoginResponseDto
            {
                Token = tokenResult.Token,
                ExpiresAt = tokenResult.ExpiresAt
            };
        }
    }
}

