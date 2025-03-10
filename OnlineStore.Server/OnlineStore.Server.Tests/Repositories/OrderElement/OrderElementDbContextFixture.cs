using OnlineStore.Server.Tests.Common;

namespace OnlineStore.Server.Tests.Repositories.OrderElement
{
    public class OrderElementDbContextFixture : DbContextFixture
    {
        public Guid OrderId_SampleA { get; private set; }
        public Guid OrderId_SampleB { get; private set; }
        public Guid ItemId_SampleA { get; private set; }
        public Guid OrderElement_ToUpdate { get; set; }
        public Guid OrderElement_ToDelete { get; set; }
        public Guid Guid_Unexists { get; private set; } = Guid.NewGuid();

        public OrderElementDbContextFixture()
        {
            Initialize();
        }

        public void Initialize(int capacity = 30)
        {
            Guid[] customersGuids = AddCustomers(capacity / 2);

            Guid[] ordersGuids = AddOrders(capacity / 2, customersGuids);

            Guid[] itemsGuids = AddItems(capacity / 2);

            Guid[] guids = AddOrderElements(capacity, ordersGuids, itemsGuids);

            OrderId_SampleA = ordersGuids[0];
            OrderId_SampleB = ordersGuids[1];

            ItemId_SampleA = itemsGuids[0];

            OrderElement_ToUpdate = guids[0];
            OrderElement_ToDelete = guids[^1];
        }
    }
}
