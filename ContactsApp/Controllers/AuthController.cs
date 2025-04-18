using ContactsApp.Entities;
using ContactsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace ContactsApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        //testowy user
        public static User user = new();

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            //hashowanie hasla
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.Email = request.Email;
            user.PasswordHash = hashedPassword;

            return Ok(user);
        }
        [HttpPost("login")]
        public ActionResult<string> Login(UserDto request)
        {
            if (user.Email != request.Email)
            {
                return BadRequest("User not found");
            }

            //sprawdzanie hasla
            var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest("Wrong password");
            }

            string token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(User user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var tokenValue = Environment.GetEnvironmentVariable("JWT_TOKEN")
                ?? configuration.GetValue<string>("AppSettings:Token")
                ?? throw new InvalidOperationException("JWT Token configuration is missing.");

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenValue));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            //tworzneie JWT
            var tokenDescriptor = new JwtSecurityToken(
                  issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                  audience: configuration.GetValue<string>("AppSettings:Audience"),
                  claims: claims,
                  expires: DateTime.UtcNow.AddDays(1),
                  signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}