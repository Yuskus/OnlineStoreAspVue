using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Validation.User
{
    public class UserValidator
    {
        public static bool CheckGuid(Guid? guid)
        {
            return guid != null && guid != Guid.Empty;
        }

        public static bool CheckManagerGuid(Guid? guid, UserRole role)
        {
            return guid == null && role == UserRole.Manager;
        }

        public static bool CheckUsername(string? name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length > 6;
        }

        public static bool CheckPassword(string? password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length > 6;
        }

        public static bool CheckPages(int pageNumber, int pageSize)
        {
            return pageNumber > 0 && pageSize > 0 && pageSize <= 36;
        }
    }
}
