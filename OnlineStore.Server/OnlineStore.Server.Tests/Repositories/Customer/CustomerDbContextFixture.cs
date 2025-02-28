using OnlineStore.Server.Tests.Common;

namespace OnlineStore.Server.Tests.Repositories.Customer
{
    public class CustomerDbContextFixture : DbContextFixture
    {
        public Guid CustomerId_ForGetById { get; private set; }
        public Guid CustomerId_ForUpdate { get; private set; }
        public Guid CustomerId_Unexist { get; private set; } = Guid.NewGuid();
        public string CustomerCode_ForGetByCode { get; private set; } = string.Empty;
        public string CustomerCode_Unexists { get; private set; } = string.Empty;
        public int CustomersTotalCount { get; private set; }

        public CustomerDbContextFixture()
        {
            Initialize();
        }

        private void Initialize(int capacity = 30)
        {
            CustomersTotalCount = capacity;

            Guid[] guids = AddCustomers(capacity);

            // some random guids for getbyid и update methods
            CustomerId_ForGetById = guids[0];
            CustomerId_ForUpdate = guids[^1];

            CustomerCode_Unexists = "9999-2000";

            // some code from user who is in the middle of the list (or unexist code if errors)
            CustomerCode_ForGetByCode = Context.Customers 
                                               .FirstOrDefault(x => x.Id == guids[capacity / 2])?
                                               .Code ?? CustomerCode_Unexists; 
        }
    }
}
