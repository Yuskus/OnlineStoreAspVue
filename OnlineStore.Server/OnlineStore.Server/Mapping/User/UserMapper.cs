using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Mapping.Customer;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Mapping.User
{
    public static class UserMapper
    {
        public static Entity.User MapToDb(this UserRequest userRequest, byte[] hash, byte[] salt)
        {
            return new()
            {
                CustomerId = userRequest.CustomerId,
                Username = userRequest.Username,
                Password = hash,
                Salt = salt,
                Role = (int)userRequest.Role
            };
        }

        public static UserResponse MapFromDb(this Entity.User userEntity)
        {
            return new()
            {
                CustomerId = userEntity.CustomerId,
                Customer = userEntity.Customer?.MapFromDb(),
                Username = userEntity.Username,
                Role = (UserRole)userEntity.Role
            };
        }

        public static void UpdateInDb(this Entity.User userEntity, UserRequest userRequest)
        {
            userEntity.Role = (int)userRequest.Role;
        }

        public static Entity.User MapAuthToDb(this RegisterRequest registerRequest, byte[] hash, byte[] salt)
        {
            return new()
            {
                CustomerId = registerRequest.CustomerId ?? Guid.NewGuid(),
                Username = registerRequest.Username,
                Password = hash,
                Salt = salt,
                Role = (int)registerRequest.Role
            };
        }

        public static LoginResponse MapAuthFromDb(this Entity.User userEntity)
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
