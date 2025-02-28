using OnlineStore.Server.Tests.Common;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Tests.Repositories.Item
{
    public class ItemDbContextFixture : DbContextFixture
    {
        public Guid ItemId_Exists { get; private set; }
        public Guid ItemId_ForUpdate { get; private set; }
        public Guid ItemId_ForDelete { get; private set; }
        public Guid ItemId_Unexists { get; private set; } = Guid.NewGuid();
        public string ItemCode_Exists { get; private set; } = string.Empty;
        public string ItemCode_Unexists { get; private set; } = "99-9999-ZZ99";
        public string ItemName_Exists { get; private set; } = string.Empty;
        public string ItemName_Unexists { get; private set; } = "Unexists";
        public string Category_SampleA { get; private set; } = string.Empty;
        public string Category_SampleB { get; private set; } = string.Empty;
        public string Category_Unexists { get; private set; } = "Unexists";
        public int ItemsTotalCount { get; private set; }

        public ItemDbContextFixture()
        {
            Initialize();
        }

        public void Initialize(int capacity = 30)
        {
            ItemsTotalCount = capacity;

            Guid[] guids = AddItems(capacity);

            var toDeleteItem = new Entity.Item
            {
                Name = "Orange",
                Category = "Fruits",
                Code = "22-3344-XI55",
                Price = 60
            };

            Context.Items.Add(toDeleteItem);
            Context.SaveChanges();
            ItemId_ForDelete = toDeleteItem.Id;
            ItemsTotalCount++;

            var someItem1 = Context.Items.FirstOrDefault(x => x.Id == guids[0]);
            var someItem2 = Context.Items.FirstOrDefault(x => x.Id == guids[1]);

            Category_SampleA = someItem1?.Category ?? Category_Unexists;
            Category_SampleB = someItem2?.Category ?? Category_Unexists;

            ItemCode_Exists = someItem1?.Code ?? ItemCode_Unexists;
            ItemName_Exists = someItem2?.Name ?? ItemName_Unexists;

            ItemId_Exists = guids[0];
            ItemId_ForUpdate = guids[^1];
        }
    }
}
