using OnlineStore.Server.DTO.Item;
using System.Text.RegularExpressions;

namespace OnlineStore.Server.Validation.Item
{
    public class ItemValidator
    {
        public static bool CheckCriteria(ItemFilterCriteria criteria)
        {
            return criteria.Id != Guid.Empty
                && CheckCategory(criteria.Category)
                && (criteria.Code is null || CheckCode(criteria.Code))
                && (criteria.Name is null || CheckName(criteria.Name));
        }

        public static bool CheckRequest(ItemRequest item)
        {
            return CheckName(item.Name)
                && CheckCategory(item.Category)
                && CheckCode(item.Code)
                && CheckPrice(item.Price);
        }

        public static bool CheckGuid(Guid? guid)
        {
            return guid != null && guid != Guid.Empty;
        }

        public static bool CheckName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length < 256;
        }

        public static bool CheckCategory(string? category)
        {
            return category is null || category.Length < 256;
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
