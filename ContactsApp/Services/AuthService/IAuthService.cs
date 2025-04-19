using ContactsApp.Entities;
using ContactsApp.Models;

namespace ContactsApp.Services.AuthService
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestsDto requests)
    }
}
