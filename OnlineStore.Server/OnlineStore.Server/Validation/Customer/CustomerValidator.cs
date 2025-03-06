using System.Text.RegularExpressions;

namespace OnlineStore.Server.Validation.Customer
{
    public class CustomerValidator
    {
        public static bool CheckGuid(Guid? guid)
        {
            return guid != null && guid != Guid.Empty;
        }

        public static bool CheckDiscount(int discount)
        {
            return discount >= 0 && discount < 100;
        }

        public static bool CheckName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length < 256;
        }

        public static bool CheckCode(string code)
        {
            if (Regex.IsMatch(code, "^[0-9]{4}-[0-9]{4}$"))
            {
                int year = int.Parse(code.Substring(5, 4));

                return year > 1900 && year <= DateOnly.FromDateTime(DateTime.Now).Year;
            }

            return false;
        }

        public static bool CheckAddress(string? address)
        {
            return address is null || address.Length < 256;
        }

        public static bool CheckPages(int pageNumber, int pageSize)
        {
            return pageNumber > 0 && pageSize > 0 && pageSize <= 50;
        }
    }
}
