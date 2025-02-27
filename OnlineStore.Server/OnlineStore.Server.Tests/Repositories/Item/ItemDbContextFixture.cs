using OnlineStore.Server.Tests.Common;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Tests.Repositories.Item
{
    public class ItemDbContextFixture : DbContextFixture
    {
        public ItemDbContextFixture()
        {
            Initialize();
        }

        public void Initialize()
        {
            var items = new Entity.Item[30];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new()
                {
                    Code = $"01-{1000 + i}-AB23",
                    Name = $"TestItemName{i}",
                    Category = $"Some Category {i % 4}",
                    Price = 100 + i
                };
            }

            Context.Items.AddRange(items);
            Context.SaveChanges();
        }
    }
}
