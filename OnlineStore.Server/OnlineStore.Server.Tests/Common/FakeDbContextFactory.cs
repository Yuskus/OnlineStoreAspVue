namespace OnlineStore.Server.Tests.Common
{
    public class FakeDbContextFactory
    {
        public static FakeDbContext Create()
        {
            var context = new FakeDbContext();
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
