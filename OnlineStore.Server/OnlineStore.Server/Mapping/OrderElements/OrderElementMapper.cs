using OnlineStore.Server.DTO.OrderElements;
using OnlineStore.Server.Mapping.Items;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Mapping.OrderElements
{
    public static class OrderElementMapper
    {
        public static OrderElement MapToDb(this OrderElementRequest orderElement)
        {
            return new()
            {
                OrderId = orderElement.OrderId,
                ItemId = orderElement.ItemId,
                ItemsCount = orderElement.ItemsCount,
                ItemPrice = orderElement.ItemPrice
            };
        }

        public static OrderElementResponse MapFromDb(this OrderElement orderElement)
        {
            return new()
            {
                Id = orderElement.Id,
                OrderId = orderElement.OrderId,
                ItemId = orderElement.ItemId,
                ItemResponse = orderElement.Item?.MapFromDb(),
                ItemsCount = orderElement.ItemsCount,
                ItemPrice = orderElement.ItemPrice
            };
        }
    }
}
