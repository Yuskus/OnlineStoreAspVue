using OnlineStore.Server.DTO.Items;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Mapping.Items
{
    public static class ItemMapper
    {
        public static Item MapToDb(this ItemRequest item)
        {
            return new()
            {
                Code = item.Code,
                Name = item.Name,
                Price = item.Price,
                Category = item.Category
            };
        }

        public static ItemResponse MapFromDb(this Item item)
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
    }
}
