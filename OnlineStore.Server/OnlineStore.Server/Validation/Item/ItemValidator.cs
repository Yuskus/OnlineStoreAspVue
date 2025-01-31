using System.Text.RegularExpressions;

namespace OnlineStore.Server.Validation.Item
{
    public class ItemValidator
    {
        public static bool CheckGuid(Guid? guid)
        {
            return guid != null && guid != Guid.Empty;
        }

        public static bool CheckCode(string code) //XX-XXXX-YYXX
        {
            if (string.IsNullOrWhiteSpace(code)) return false;

            if (code.Length != 12) return false;

            return Regex.IsMatch(code, "^[0-9]{2}-[0-9]{4}-[A-Z]{2}[0-9]{2}$");
        }

        public static bool CheckPrice(double? price)
        {
            return price == null || price > 0;
        }

        public static bool CheckPages(int pageNumber, int pageSize)
        {
            return pageNumber > 0 && pageSize > 0 && pageSize <= 36;
        }
    }
}
