namespace ContactsApp.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        // hashowane hasło jest przechowywane w bazie danych a nie w postaci czystego tekstu
        public string PasswordHash { get; set; } = string.Empty;
        // refresh token dla bezpieczenstwa
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}