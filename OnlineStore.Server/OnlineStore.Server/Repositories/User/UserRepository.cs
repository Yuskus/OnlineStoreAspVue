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

        public async Task<bool> RegisterUser(RegisterRequest registerRequest)
        {
            if (await _context.Users.AnyAsync(x => x.Username == registerRequest.Username)) return false;

            (byte[] hash, byte[] salt) = Hasher.CreatePasswordHash(registerRequest.Password);

            Entity.User userEntity = registerRequest.MapAuthToDb(hash, salt);

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUser(UserRequest userRequest)
        {
            Entity.User? user = await _context.Users.FirstOrDefaultAsync(x => x.Username == userRequest.Username);

            if (user is null) return false;

            user.Role = (int)userRequest.Role;
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

        public async Task<IEnumerable<UserResponse>> GetPageOfUsersInfo(int pageNumber, int pageSize)
        {
            List<UserResponse> result = await _context.Users.Skip((pageNumber - 1) * pageSize)
                                                            .Take(pageSize)
                                                            .Include(x => x.Customer)
                                                            .Select(x => x.MapFromDb())
                                                            .ToListAsync();

            return result;
        }
    }
}
