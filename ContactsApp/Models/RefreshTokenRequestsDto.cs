using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Models
{
    public class RefreshTokenRequestsDto
    {
        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Refresh token is required.")]
        public required string RefreshToken { get; set; }
    }
}
