using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.DTO.Users
{
    public class UserRequest
    {
        public required string Username { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }
}
