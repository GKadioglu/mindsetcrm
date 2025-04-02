using Microsoft.AspNetCore.Mvc;
using UserService.Application.Constants;
using UserService.Application.Interfaces;
using UserService.Domain.DTOs;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            try
            {
                var userDto = await _userService.RegisterAsync(registerDto);
                return Ok(new
                {
                    message = SuccessMessages.UserCreated,
                    user = userDto
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var loginResponse = await _userService.LoginAsync(loginDto);
                return Ok(new
                {
                    message = SuccessMessages.LoginSuccessful,
                    data = loginResponse
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}
