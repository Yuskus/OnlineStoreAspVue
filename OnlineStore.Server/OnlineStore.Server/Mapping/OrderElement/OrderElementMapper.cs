using OnlineStore.Server.DTO.OrderElement;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Mapping.OrderElement
{
    public static class OrderElementMapper
    {
        public static Entity.OrderElement MapToDb(this OrderElementRequest orderElement)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                OrderId = orderElement.OrderId,
                ItemId = orderElement.ItemId,
                ItemsCount = orderElement.ItemsCount,
                ItemPrice = orderElement.ItemPrice
            };
        }

        public static OrderElementResponse MapFromDb(this Entity.OrderElement orderElement)
        {
            return new()
            {
                Id = orderElement.Id,
                OrderId = orderElement.OrderId,
                ItemId = orderElement.ItemId,
                ItemsCount = orderElement.ItemsCount,
                ItemPrice = orderElement.ItemPrice
            };
        }

        public static void UpdateInDb(this Entity.OrderElement orderElementEntity, OrderElementRequest orderElement)
        {
            orderElementEntity.OrderId = orderElement.OrderId;
            orderElementEntity.ItemId = orderElement.ItemId;
            orderElementEntity.ItemsCount = orderElement.ItemsCount;
            orderElementEntity.ItemPrice = orderElement.ItemPrice;
        }
    }
}
