using OnlineStore.Server.DTO.Item;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Mapping.Item
{
    public static class ItemMapper
    {
        public static Entity.Item MapToDb(this ItemRequest item)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Code = item.Code,
                Name = item.Name,
                Price = item.Price,
                Category = item.Category
            };
        }

        public static ItemResponse MapFromDb(this Entity.Item item)
        {
            return new()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Price = item.Price,
                Category = item.Category
            };
        }

        public static void UpdateInDb(this Entity.Item itemEntity, ItemRequest item)
        {
            itemEntity.Code = item.Code;
            itemEntity.Name = item.Name;
            itemEntity.Price = item.Price;
            itemEntity.Category = item.Category;
        }
    }
}
