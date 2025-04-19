using ContactsApp.Data;
using ContactsApp.Entities;
using ContactsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ContactsApp.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<TokenResponseDto?> LoginAsync(UserDto request)
        {
            //walidacja uzytkownika
            ValidateUserDto(request);

            //znalezienie usera
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user is null)
            {
                return null;
            }

            //weryfikacja hasla
            var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokensResponseAsync(user);
        }

        public async Task<User?> RegisterAsync(UserDto request)
        {
            //walidacja inputa
            ValidateUserDto(request);

            //sprawdzenie czy uzytkownik istnieje
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return null;
            }

            //stworzenie nowego uzytkownika
            var user = new User
            {
                Email = request.Email
            };

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestsDto request)
        {
            //walidacja inputa
            if (request is null || request.UserId == Guid.Empty || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return null;
            }

            //walidacja tokenu
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
            {
                return null;
            }

            return await CreateTokensResponseAsync(user);
        }

        private async Task<TokenResponseDto> CreateTokensResponseAsync(User user)
        {
            //generowanie tokenow
            var accessToken = CreateAccessToken(user);
            var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            //znalezienie uzytkownika i sprawdzenie tokenu
            var user = await _context.Users.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }

        private string GenerateRefreshToken()
        {
            //generowanie losowego tokenu
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            //generowanie i zapisanie tokenu
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        private string CreateAccessToken(User user)
        {
            //stworznie JWT claimow
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            //config JWT
            var tokenValue = Environment.GetEnvironmentVariable("JWT_TOKEN")
                ?? _configuration.GetValue<string>("AppSettings:Token")
                ?? throw new InvalidOperationException("JWT Token configuration is missing.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValue));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            //stworzenie JWT
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private void ValidateUserDto(UserDto request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Email and password are required.");
            }

            if (!Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException("Invalid email format.");
            }

            if (!IsPasswordValid(request.Password))
            {
                throw new ArgumentException("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
            }
        }

        private bool IsPasswordValid(string password)
        {
            return password.Length >= 8 &&
                   Regex.IsMatch(password, @"[A-Z]") &&      //przynajmniej jedna duża litera
                   Regex.IsMatch(password, @"[a-z]") &&      //przynajmniej jedna mała litera
                   Regex.IsMatch(password, @"[0-9]") &&      //przynajmniej jedna cyfra
                   Regex.IsMatch(password, @"[\W_]");        //przynajmniej jeden znak specjalny
        }
    }
}