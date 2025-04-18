namespace ContactsApp.Models
{
    public class RefreshTokenRequestsDto
    {
        public Guid UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
