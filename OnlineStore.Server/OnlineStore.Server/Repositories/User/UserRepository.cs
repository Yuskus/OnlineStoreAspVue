using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Authorization.Utilities;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Mapping.User;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Repositories.User
{
    public class UserRepository(OnlineStoreDbContext context, IConfiguration configuration, ILogger<UserRepository> logger) : IUserRepository
    {
        private readonly OnlineStoreDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<UserRepository> _logger = logger;

        public async Task<LoginResponse?> Authenticate(UserCredentialsRequest loginRequest)
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

                _logger.LogWarning("Неудачная попытка входа!");
            }

            return null;
        }

        public async Task<bool> RegisterUser(UserCredentialsRequest registerRequest)
        {
            if (await _context.Users.AnyAsync(x => x.Username == registerRequest.Username)) return false;

            (byte[] hash, byte[] salt) = Hasher.CreatePasswordHash(registerRequest.Password);

            Entity.User userEntity = registerRequest.MapUserToDb(hash, salt);

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUser(string username, UserRequest userRequest)
        {
            Entity.User? userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (userEntity is null) return false;

            userEntity.UpdateInDb(userRequest); // меняем только роль и логин (логика смены пароля не добавлена)
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUser(string username)
        {
            Entity.User? user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user is null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ResponseList<UserResponse>> GetPageOfUsersInfo(int pageNumber, int pageSize)
        {
            List<UserResponse> response = await _context.Users.Skip((pageNumber - 1) * pageSize)
                                                              .Take(pageSize)
                                                              .Include(x => x.Customer)
                                                              .Select(x => x.MapFromDb())
                                                              .ToListAsync();

            int totalCount = await _context.Users.CountAsync();

            ResponseList<UserResponse> result = new(response, totalCount);

            return result;
        }
    }
}
