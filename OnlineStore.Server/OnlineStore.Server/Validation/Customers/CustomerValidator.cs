using OnlineStore.Server.DTO.Customers;
using System.Text.RegularExpressions;

namespace OnlineStore.Server.Validation.Customers
{
    public static partial class CustomerValidator
    {
        private static readonly int _minValidYear = 1900;
        private static readonly int _nameLength = 256;
        private static readonly int _addressLength = 256;
        private static readonly int _maxPageSize = 50;
        private static readonly int _maxDiscount = 70;

        public static bool CheckCriteria(CustomerFilterCriteria criteria)
        {
            bool validId = criteria.Id != Guid.Empty;
            bool validCode = criteria.Code is null || CheckCode(criteria.Code);

            return validId && validCode;
        }

        public static bool CheckRequest(CustomerBaseRequest customer)
        {
            bool isValid =
                CheckName(customer.Name) &&
                CheckCode(customer.Code) &&
                CheckAddress(customer.Address);

            return isValid;
        }

        public static bool CheckRequest(CustomerRequest customer)
        {
            bool isValid =
                CheckName(customer.Name) &&
                CheckCode(customer.Code) &&
                CheckAddress(customer.Address) &&
                CheckDiscount(customer.Discount);

            return isValid;
        }

        public static bool CheckGuid(Guid? guid)
        {
            return guid != null && guid != Guid.Empty;
        }

        public static bool CheckDiscount(int discount)
        {
            return discount >= 0 && discount < _maxDiscount;
        }

        public static bool CheckName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            return name.Length < _nameLength;
        }

        public static bool CheckCode(string code)
        {
            if (IsCode().IsMatch(code))
            {
                int codeYear = int.Parse(code.Substring(5, 4));
                int nowYear = DateOnly.FromDateTime(DateTime.Now).Year;

                return
                    codeYear > _minValidYear &&
                    codeYear <= nowYear;
            }

            return false;
        }

        public static bool CheckAddress(string? address)
        {
            if (address is null) return true;

            return
                address.Length > 0 &&
                address.Length < _addressLength;
        }

        public static bool CheckPages(int pageNumber, int pageSize)
        {
            return
                pageNumber > 0 &&
                pageSize > 0 &&
                pageSize <= _maxPageSize;
        }
    }

    // regexes
    public static partial class CustomerValidator
    {
        [GeneratedRegex("^[0-9]{4}-[0-9]{4}$")]
        private static partial Regex IsCode();
    }
}
