﻿using OnlineStore.Server.DTO.Customer;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Mapping.Customer
{
    public static class CustomerMapper
    {
        public static Entity.Customer MapToDb(this CustomerBaseRequest customer)
        {
            Entity.Customer mapped = new()
            {
                Id = Guid.NewGuid(),
                Name = customer.Name,
                Code = customer.Code,
                Address = customer.Address
            };

            if (customer is CustomerRequest customerRequest)
            {
                mapped.Discount = customerRequest.Discount;
            }

            return mapped;
        }

        public static CustomerResponse MapFromDb(this Entity.Customer customer)
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

        public static void UpdateInDb(this Entity.Customer customerEntity, CustomerRequest customer)
        {
            customerEntity.Name = customer.Name;
            customerEntity.Code = customer.Code;
            customerEntity.Address = customer.Address;
            customerEntity.Discount = customer.Discount;
        }
    }
}
