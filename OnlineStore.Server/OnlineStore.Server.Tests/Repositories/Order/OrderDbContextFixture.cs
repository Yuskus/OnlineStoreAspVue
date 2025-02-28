using OnlineStore.Server.Tests.Common;

namespace OnlineStore.Server.Tests.Repositories.Order
{
    public class OrderDbContextFixture : DbContextFixture
    {
        public Guid CustomerId_SampleA { get; private set; }
        public Guid CustomerId_SampleB { get; private set; }
        public Guid Guid_Unexists { get; private set; } = Guid.NewGuid();
        public string Status_New { get; private set; } = "new";
        public string Status_Basket { get; private set; } = "basket";
        public string Status_Unexists { get; private set; } = "Unexists";
        public int OrderNumber_Exists { get; private set; } = 0;
        public int OrderNumber_Unexists { get; private set; } = int.MaxValue;
        public Guid OrderId_StatusBasket { get; private set; }
        public Guid OrderId_StatusNew { get; private set; }
        public int OrdersTotalCount { get; private set; }
        public Guid OrderId_ForUpdate { get; private set; }
        public Guid OrderId_ForDelete { get; private set; }

        public OrderDbContextFixture()
        {
            Initialize();
        }

        public void Initialize(int capacity = 30)
        {
            OrdersTotalCount = capacity;

            Guid[] customerGuids = AddCustomers(5);

            Guid[] ordersGuids = AddOrders(capacity, customerGuids);

            CustomerId_SampleA = customerGuids[0];
            CustomerId_SampleB = customerGuids[^1];

            OrderId_StatusNew = ordersGuids[0];
            OrderId_StatusBasket = ordersGuids[1];

            OrderId_ForUpdate = ordersGuids[^1];
            OrderId_ForDelete = ordersGuids[^2];
        }
    }
}
