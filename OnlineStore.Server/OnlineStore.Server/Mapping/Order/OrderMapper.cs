using OnlineStore.Server.DTO.Order;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Mapping.Order
{
    public static class OrderMapper
    {
        public static Entity.Order MapToDb(this OrderRequest order, int orderNumber)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                CustomerId = order.CustomerId,
                OrderDate = DateOnly.Parse(order.OrderDate),
                ShipmentDate = ParseDateOrNull(order.ShipmentDate),
                OrderNumber = orderNumber,
                OrderStatus = order.OrderStatus
            };
        }

        public static OrderResponse MapFromDb(this Entity.Order order)
        {
            return new()
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                ShipmentDate = order.ShipmentDate,
                OrderNumber = order.OrderNumber,
                OrderStatus = order.OrderStatus
            };
        }

        public static void UpdateInDb(this Entity.Order orderEntity, OrderRequest order)
        {
            orderEntity.CustomerId = order.CustomerId;
            orderEntity.OrderDate = DateOnly.Parse(order.OrderDate);
            orderEntity.ShipmentDate = ParseDateOrNull(order.ShipmentDate);
            orderEntity.OrderStatus = order.OrderStatus;
        }

        private static DateOnly? ParseDateOrNull(string? value)
        {
            if (value is null) return null;
            return DateOnly.Parse(value);
        }
    }
}
