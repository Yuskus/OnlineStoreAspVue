using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.Customers;

namespace OnlineStore.Server.DTO.Users
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public CustomerResponse? Customer { get; set; }
        public required string Username { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }
}
