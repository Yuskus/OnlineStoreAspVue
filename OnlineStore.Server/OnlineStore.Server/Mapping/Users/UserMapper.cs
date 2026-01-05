using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.Users;
using OnlineStore.Server.Mapping.Customers;

namespace OnlineStore.Server.Mapping.Users
{
    public static class UserMapper
    {
        public static UserResponse MapFromDb(this User userEntity)
        {
            return new()
            {
                Id = userEntity.Id,
                Customer = userEntity.Customer?.MapFromDb(),
                Username = userEntity.Username,
                Role = userEntity.Role
            };
        }

        public static User MapToDb(this UserCredentialsRequest registerRequest, byte[] hash, byte[] salt)
        {
            return new()
            {
                Username = registerRequest.Username,
                Password = hash,
                Salt = salt,
                Role = UserRole.Manager
            };
        }

        public static User MapToDb(this CustomerRegisterRequest registerRequest, byte[] hash, byte[] salt)
        {
            return new()
            {
                Username = registerRequest.Username,
                Password = hash,
                Salt = salt,
                Role = UserRole.User,
                CustomerId = registerRequest.Id
            };
        }
    }
}
