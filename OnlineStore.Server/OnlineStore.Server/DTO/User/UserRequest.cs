using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.DTO.User
{
    public class UserRequest
    {
        public required string Username { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }
}
