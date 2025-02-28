using OnlineStore.Server.Tests.Common;

namespace OnlineStore.Server.Tests.Repositories.OrderElement
{
    public class OrderElementDbContextFixture : DbContextFixture
    {
        public int OrderElementsTotalCount { get; private set; }
        public OrderElementDbContextFixture()
        {
            Initialize();
        }

        public void Initialize(int capacity = 30)
        {
            OrderElementsTotalCount = capacity;

            Guid[] customersGuids = AddCustomers(capacity / 2);

            Guid[] ordersGuids = AddOrders(capacity / 2, customersGuids);

            Guid[] itemsGuids = AddItems(capacity / 2);

            Guid[] _ = AddOrderElements(capacity, ordersGuids, itemsGuids);
        }
    }
}
