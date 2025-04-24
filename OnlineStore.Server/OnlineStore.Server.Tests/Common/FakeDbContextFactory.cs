using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;

namespace OnlineStore.Server.Tests.Common
{
    public static class FakeDbContextFactory
    {
        public static FakeDbContext Create()
        {
            string databaseUniqName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<OnlineStoreDbContext>().UseInMemoryDatabase(databaseUniqName).Options;
            var context = new FakeDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        public static void Delete(FakeDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
