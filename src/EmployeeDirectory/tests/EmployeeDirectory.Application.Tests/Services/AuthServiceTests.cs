using EmployeeDirectory.Application.DTOs;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Application.Services;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Tests.Services
{
    public class AuthServiceTests
    {
        //mock the IUserRepository and use a real PasswordHasher<AppUser> to test the ValidateCredentialsAsync method of the AuthService class
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly PasswordHasher<AppUser> _passwordHasher;
        private readonly AuthService _service;
        public AuthServiceTests() 
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasher = new PasswordHasher<AppUser>();
            _service = new AuthService(_userRepositoryMock.Object, _passwordHasher);


        }

        [Fact]
        public async Task ValidateCredentialsAsync_WithCorrectPassword_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var password = "CorrectPassword123";

            var user = new AppUser
            {
                Email = email,
                PasswordHash = _passwordHasher.HashPassword(null!, password)
            };

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync(user);

            var loginRequest = new LoginRequestDto
            {
                Email = email,
                Password = password
            };

            // Act
            var result = await _service.ValidateCredentialsAsync(loginRequest);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_WithWrongPassword_ReturnsFalse()
        {
            // Arrange
            var email = "test@example.com";
            var correctPassword = "CorrectPassword123";
            var wrongPassword = "WrongPassword";

            var user = new AppUser
            {
                Email = email,
                PasswordHash = _passwordHasher.HashPassword(null!, correctPassword)
            };

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync(user);

            var loginRequest = new LoginRequestDto
            {
                Email = email,
                Password = wrongPassword
            };

            // Act
            var result = await _service.ValidateCredentialsAsync(loginRequest);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_WithUnknownUser_ReturnsFalse()
        {
            // Arrange
            var email = "unknown@example.com";

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync((AppUser?)null);

            var loginRequest = new LoginRequestDto
            {
                Email = email,
                Password = "AnyPassword"
            };

            // Act
            var result = await _service.ValidateCredentialsAsync(loginRequest);

            // Assert
            Assert.False(result);
        }
    }
}
