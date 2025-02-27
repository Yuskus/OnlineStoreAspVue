using OnlineStore.Server.Tests.Common;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Tests.Repositories.Order
{
    public class OrderDbContextFixture : DbContextFixture
    {
        public OrderDbContextFixture()
        {
            Initialize();
        }

        public void Initialize()
        {
            var orders = new Entity.Order[30];

            Guid[] guids = [.. Context.Customers.Take(5).Select(x => x.Id)];

            for (int i = 0; i < orders.Length; i++)
            {
                orders[i] = new()
                {
                    CustomerId = guids[i % 5],
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    OrderNumber = i
                };
            }

            Context.Orders.AddRange(orders);
            Context.SaveChanges();
        }
    }
}
