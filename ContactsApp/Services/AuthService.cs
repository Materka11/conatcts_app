using ContactsApp.Data;
using ContactsApp.Entities;
using ContactsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ContactsApp.Services
{
    public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<string?> LoginAsync(UserDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user is null)
            {
                return null;
            }

            //sprawdzanie hasla
            var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return CreateToken(user);
        }

        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return null;
            }

            var user = new User();
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.Email = request.Email;
            user.PasswordHash = hashedPassword;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
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
