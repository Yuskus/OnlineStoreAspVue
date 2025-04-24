using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;

namespace OnlineStore.Server.Tests.Common
{
    public class FakeDbContext(DbContextOptions<OnlineStoreDbContext> options) : OnlineStoreDbContext(options) { }
}
