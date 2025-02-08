namespace OnlineStore.Server.Validation.User
{
    public class UserValidator
    {
        public static bool CheckUsername(string? name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Trim().Length > 6;
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
