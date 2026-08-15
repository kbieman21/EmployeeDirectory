using Microsoft.AspNetCore.Mvc;
using EmployeeDirectory.Application.DTOs;
using EmployeeDirectory.Application.Interfaces;

namespace EmployeeDirectory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var isValid =
            await _authService.ValidateCredentialsAsync(loginRequest);

            if (!isValid)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            return Ok(new
            {
                message = "Login successful."
            });
        }
    }
}
