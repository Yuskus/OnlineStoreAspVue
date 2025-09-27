using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Validation.User
{
    public static class UserValidator
    {
        public static bool CheckCredentials(UserCredentialsRequest request)
        {
            return CheckUsername(request.Username)
                && CheckPassword(request.Password);
        }

        public static bool CheckCredentials(string username, string password)
        {
            return CheckUsername(username)
                && CheckPassword(password);
        }

        public static bool CheckUsername(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            string trimmed = name.Trim();

            return trimmed.Length > 6 && trimmed.Length < 100;
        }

        public static bool CheckPassword(string? password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length > 6 && password.Length < 100;
        }

        public static bool CheckPages(int pageNumber, int pageSize)
        {
            return pageNumber > 0 && pageSize > 0 && pageSize <= 36;
        }
    }
}
