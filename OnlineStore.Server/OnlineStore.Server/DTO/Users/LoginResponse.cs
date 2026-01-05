using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.DTO.Users
{
    public class LoginResponse
    {
        public Guid? CustomerId { get; set; }
        public required string Username { get; set; }
        public UserRole Role { get; set; }
        public string? Token { get; set; }
    }
}
