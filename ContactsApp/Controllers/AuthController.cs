using ContactsApp.Entities;
using ContactsApp.Models;
using ContactsApp.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _authService.RegisterAsync(request);
                if (user == null)
                {
                    return BadRequest("User with this email already exists.");
                }

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error during registration for email: {Email}", request?.Email);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for email: {Email}", request?.Email);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] UserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.LoginAsync(request);
                if (result == null)
                {
                    return BadRequest("Invalid credentials.");
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error during login for email: {Email}", request?.Email);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for email: {Email}", request?.Email);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken([FromBody] RefreshTokenRequestsDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.RefreshTokensAsync(request);
                if (result == null || result.AccessToken == null || result.RefreshToken == null)
                {
                    return Unauthorized("Invalid refresh token.");
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error during refresh token for user ID: {UserId}", request?.UserId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during refresh token for user ID: {UserId}", request?.UserId);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}