using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;

namespace OnlineStore.Server.Tests.Common
{
    public class FakeDbContext : OnlineStoreDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }
    }
}
