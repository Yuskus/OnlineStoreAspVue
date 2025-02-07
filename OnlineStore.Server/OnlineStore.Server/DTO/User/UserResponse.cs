using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.Customer;

namespace OnlineStore.Server.DTO.User
{
    public class UserResponse
    {
        public int Id { get; set; }
        public CustomerResponse? Customer { get; set; }
        public required string Username { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }
}
