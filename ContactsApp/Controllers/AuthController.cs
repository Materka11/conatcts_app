using ContactsApp.Entities;
using ContactsApp.Models;
using ContactsApp.Services;
using Microsoft.AspNetCore.Mvc;


namespace ContactsApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        //testowy user
        public static User user = new();

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);

            if (user is null)
            {
                return BadRequest("User with this email already exists");
            }

            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var token = await authService.LoginAsync(request);

            if (token is null)
            {
                return BadRequest("Invalid credentials");
            }

            return Ok(token);
        }


    }
}