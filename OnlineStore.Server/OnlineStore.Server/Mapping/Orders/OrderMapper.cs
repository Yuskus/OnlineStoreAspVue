using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.Orders;

namespace OnlineStore.Server.Mapping.Orders
{
    public static class OrderMapper
    {
        public static Order MapToDb(this OrderRequest order)
        {
            return new()
            {
                CustomerId = order.CustomerId,
                OrderDate = DateOnly.Parse(order.OrderDate),
                ShipmentDate = DateOnly.TryParse(order.ShipmentDate, out DateOnly date) ? date : null,
                OrderStatus = order.OrderStatus
            };
        }

        public static OrderResponse MapFromDb(this Order order)
        {
            return new()
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer?.Name,
                OrderDate = order.OrderDate,
                ShipmentDate = order.ShipmentDate,
                OrderNumber = order.OrderNumber,
                OrderStatus = order.OrderStatus
            };
        }
    }
}
