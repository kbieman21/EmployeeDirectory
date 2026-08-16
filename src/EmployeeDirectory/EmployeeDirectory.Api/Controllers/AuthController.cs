using Azure.Core;
using EmployeeDirectory.Application.DTOs;
using EmployeeDirectory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            //var isValid =
            //await _authService.ValidateCredentialsAsync(loginRequest);
            //CHANGING THE ABOVE CODE TO BELOW BECAUSE WE MOVED TO JWT
            var loginResponse = await _authService.LoginAsync(loginRequest);

            if (loginResponse is null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            return Ok(loginResponse);
        }
    }
}
