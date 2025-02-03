using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.DTO.User
{
    public class UserRequest
    {
        public Guid? CustomerId { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }
}
