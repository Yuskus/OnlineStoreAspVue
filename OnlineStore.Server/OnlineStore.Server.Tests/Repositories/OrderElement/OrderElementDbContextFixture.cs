using OnlineStore.Server.Tests.Common;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Tests.Repositories.OrderElement
{
    public class OrderElementDbContextFixture : DbContextFixture
    {
        public OrderElementDbContextFixture()
        {
            Initialize();
        }

        public void Initialize()
        {
            var elements = new Entity.OrderElement[30];

            Guid[] ordersGuids = [.. Context.Orders.Take(7).Select(x => x.Id)];
            Guid[] itemsGuids = [.. Context.Items.Take(10).Select(x => x.Id)];
            double?[] itemsPrices = [.. Context.Items.Take(10).Select(x => x.Price)];

            for (int i = 0; i < elements.Length; i++)
            {
                elements[i] = new()
                {
                    OrderId = ordersGuids[i % 7],
                    ItemId = itemsGuids[i % 10],
                    ItemsCount = i % 3,
                    ItemPrice = 100 + i
                };
            }

            Context.OrderElements.AddRange(elements);
            Context.SaveChanges();
        }
    }
}
