using EmployeeDirectory.Application.DTOs;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Application.Models;
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
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly PasswordHasher<AppUser> _passwordHasher;
        private readonly AuthService _service;
        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _passwordHasher = new PasswordHasher<AppUser>();

            _service = new AuthService(
                _userRepositoryMock.Object,
                _tokenServiceMock.Object,
                _passwordHasher);
        }

        [Fact]
        public async Task LoginAsync_WithCorrectPassword_ReturnsNotNull()
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

            var expiresAt =
    DateTime.UtcNow.AddHours(1);

            _tokenServiceMock
                .Setup(service =>
                    service.CreateToken(user))
                .Returns(new TokenResult
                {
                    Token = "test-jwt-token",
                    ExpiresAt = expiresAt
                });
            // Act
            var result = await _service.LoginAsync(loginRequest);// ValidateCredentialsAsync(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test-jwt-token", result.Token);
            //Assert.True(result.ExpiresAt > DateTime.UtcNow);
            Assert.Equal(expiresAt, result.ExpiresAt);
        }

        [Fact]
        public async Task LoginAsync_WithWrongPassword_ReturnsNull()
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
            var result = await _service.LoginAsync(loginRequest);// ValidateCredentialsAsync(loginRequest);

            // Assert
            //Assert.False(result);
            //Assert.NotNull(result);
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_WithUnknownUser_ReturnsNull()
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
            var result = await _service.LoginAsync(loginRequest);// ValidateCredentialsAsync(loginRequest);

            // Assert
            // Assert.False(result);
            //Assert.NotNull(result);
            Assert.Null(result);
        }
    }
}
