using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.DTO.User
{
    public class LoginResponse
    {
        public required string Username { get; set; }
        public UserRole Role { get; set; }
        public string? Token { get; set; }
    }
}
