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
                Role = (UserRole)userEntity.Role
            };
        }

        public static void UpdateInDb(this User userEntity, UserRequest userRequest)
        {
            userEntity.Username = userRequest.Username;
            userEntity.Role = (int)userRequest.Role;
        }

        public static User MapCustomerToDb(this CustomerRegisterRequest registerRequest, Guid id, byte[] hash, byte[] salt)
        {
            return new()
            {
                CustomerId = id,
                Username = registerRequest.Username,
                Password = hash,
                Salt = salt,
                Role = (int)UserRole.User
            };
        }

        public static User MapManagerToDb(this UserCredentialsRequest registerRequest, byte[] hash, byte[] salt)
        {
            return new()
            {
                Username = registerRequest.Username,
                Password = hash,
                Salt = salt,
                Role = (int)UserRole.Manager
            };
        }

        public static User MapUserToDb(this UserCredentialsRequest registerRequest, byte[] hash, byte[] salt)
        {
            return new()
            {
                Username = registerRequest.Username,
                Password = hash,
                Salt = salt,
                Role = (int)UserRole.Manager
            };
        }

        public static User MapUserToDb(this CustomerRegisterRequest registerRequest, byte[] hash, byte[] salt)
        {
            return new()
            {
                Username = registerRequest.Username,
                Password = hash,
                Salt = salt,
                Role = (int)UserRole.User,
                CustomerId = registerRequest.Id
            };
        }

        public static LoginResponse MapAuthFromDb(this User userEntity)
        {
            return new()
            {
                CustomerId = userEntity.CustomerId,
                Username = userEntity.Username,
                Role = (UserRole)userEntity.Role
            };
        }
    }
}
