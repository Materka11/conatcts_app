using ContactsApp.Entities;
using ContactsApp.Models;

namespace ContactsApp.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<string?> LoginAsync(UserDto request);
    }
}
