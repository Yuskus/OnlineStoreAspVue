using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Authorization.Utilities;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Mapping.User;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Repositories.User
{
    public class UserRepository(OnlineStoreDbContext context, IConfiguration configuration) : IUserRepository
    {
        private readonly OnlineStoreDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        public async Task<LoginResponse?> Authenticate(LoginRequest loginRequest)
        {
            Entity.User? user = await _context.Users.FirstOrDefaultAsync(x => x.Username == loginRequest.Username);

            if (user is not null)
            {
                bool isValid = Hasher.IsPasswordValid(loginRequest.Password, user.Password, user.Salt);

                if (isValid)
                {
                    LoginResponse response = user.MapAuthFromDb();

                    TokenGenerator.GenerateJwtToken(_configuration, response);

                    return response;
                }
            }

            return null;
        }

        public async Task<bool> RegisterManager(ManagerRegisterRequest managerRegisterRequest)
        {
            if (await _context.Users.AnyAsync(x => x.Username == managerRegisterRequest.Username)) return false;

            (byte[] hash, byte[] salt) = Hasher.CreatePasswordHash(managerRegisterRequest.Password);

            Entity.User userEntity = managerRegisterRequest.MapManagerToDb(hash, salt);

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RegisterUser(Guid customerId, CustomerRegisterRequest customerRegisterRequest)
        {
            if (await _context.Users.AnyAsync(x => x.Username == customerRegisterRequest.Username)) return false;

            (byte[] hash, byte[] salt) = Hasher.CreatePasswordHash(customerRegisterRequest.Password);

            Entity.User userEntity = customerRegisterRequest.MapCustomerToDb(customerId, hash, salt);

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUser(Guid id, UserRequest userRequest)
        {
            Entity.User? user = await _context.Users.FirstOrDefaultAsync(x => x.CustomerId == id);

            if (user is null) return false;

            user.Role = (int)userRequest.Role; // обновляем пока только роль
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUser(string username)
        {
            Entity.User? user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user is null) return false;

            if (user.Customer != null)

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserResponseList> GetPageOfUsersInfo(int pageNumber, int pageSize)
        {
            List<UserResponse> response = await _context.Users.Skip((pageNumber - 1) * pageSize)
                                                              .Take(pageSize)
                                                              .Include(x => x.Customer)
                                                              .Select(x => x.MapFromDb())
                                                              .ToListAsync();

            int totalCount = await _context.Users.CountAsync();

            UserResponseList result = new(response, totalCount);

            return result;
        }
    }
}
