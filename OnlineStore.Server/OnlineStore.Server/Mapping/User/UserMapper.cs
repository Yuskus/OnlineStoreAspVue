using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Mapping.Customer;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Mapping.User
{
    public static class UserMapper
    {
        public static UserResponse MapFromDb(this Entity.User userEntity)
        {
            return new()
            {
                Id = userEntity.Id,
                Customer = userEntity.Customer?.MapFromDb(),
                Username = userEntity.Username,
                Role = (UserRole)userEntity.Role
            };
        }

        public static void UpdateInDb(this Entity.User userEntity, UserRequest userRequest)
        {
            userEntity.Username = userRequest.Username;
            userEntity.Role = (int)userRequest.Role;
        }

        public static Entity.User MapCustomerToDb(this CustomerRegisterRequest registerRequest, Guid id, byte[] hash, byte[] salt)
        {
            return new()
            {
                CustomerId = id,
                Username = registerRequest.Username,
                Password = hash,
                Salt = salt,
                Role = 0 //тк Customer
            };
        }

        public static Entity.User MapManagerToDb(this ManagerRegisterRequest registerRequest, byte[] hash, byte[] salt)
        {
            return new()
            {
                Username = registerRequest.Username,
                Password = hash,
                Salt = salt,
                Role = 1 //тк Manager
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
