using OnlineStore.Server.DTO.Customers;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Mapping.Customers
{
    public static class CustomerMapper
    {
        public static Customer MapToDb(this CustomerBaseRequest customer)
        {
            return new()
            {
                Name = customer.Name,
                Code = customer.Code,
                Address = customer.Address,
                Discount = 0
            };
        }

        public static Customer MapToDb(this CustomerRequest customer)
        {
            return new()
            {
                Name = customer.Name,
                Code = customer.Code,
                Address = customer.Address,
                Discount = customer.Discount
            };
        }

        public static CustomerResponse MapFromDb(this Customer customer)
        {
            return new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Code = customer.Code,
                Address = customer.Address,
                Discount = customer.Discount
            };
        }
    }
}
