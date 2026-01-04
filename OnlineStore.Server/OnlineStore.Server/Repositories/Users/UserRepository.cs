using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Authorization.Utilities;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Users;
using OnlineStore.Server.Mapping.Users;

namespace OnlineStore.Server.Repositories.Users
{
    public class UserRepository(
        OnlineStoreDbContext context,
        IConfiguration configuration) : IUserRepository
    {
        private readonly OnlineStoreDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        public async Task<LoginResponse?> Authenticate(UserCredentialsRequest loginRequest)
        {
            if (await _context.Users.FirstOrDefaultAsync(x => x.Username == loginRequest.Username) is User user)
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

        public async Task<bool> CreateUserIfNotExists(UserCredentialsRequest registerRequest)
        {
            if (await _context.Users.AnyAsync(x => x.Username == registerRequest.Username)) return false;

            (byte[] hash, byte[] salt) = Hasher.CreatePasswordHash(registerRequest.Password);

            User userEntity = registerRequest.MapUserToDb(hash, salt);

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateCustomerIfNotExists(CustomerRegisterRequest registerRequest)
        {
            if (await _context.Users.AnyAsync(x => x.Username == registerRequest.Username)) return false;

            (byte[] hash, byte[] salt) = Hasher.CreatePasswordHash(registerRequest.Password);

            User userEntity = registerRequest.MapUserToDb(hash, salt);

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(string username, UserRequest userRequest)
        {
            if (await _context.Users.FirstOrDefaultAsync(x => x.Username == username) is User user)
            {
                user.UpdateInDb(userRequest); // меняем только роль и логин (логика смены пароля не добавлена)
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> Delete(string username)
        {
            if (await _context.Users.FirstOrDefaultAsync(x => x.Username == username) is User user)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                
                return true;
            }

            return false;
        }

        public async Task<ResponseList<UserResponse>> GetPage(PageInfo pageInfo)
        {
            var query = _context.Users
                .Include(x => x.Customer)
                .AsSingleQuery();

            return new()
            {
                Responses = await query
                    .Skip((pageInfo.Number - 1) * pageInfo.Size)
                    .Take(pageInfo.Size)
                    .Select(x => x.MapFromDb())
                    .ToListAsync(),
                TotalCount = await query
                    .CountAsync()
            };
        }
    }
}
