namespace OnlineStore.Server.Tests.Common
{
    public class DbContextFixture : IDisposable
    {
        public FakeDbContext Context { get; set; }
        private bool disposed = false;

        public DbContextFixture()
        {
            Context = FakeDbContextFactory.Create();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                FakeDbContextFactory.Delete(Context);
            }
            disposed = true;
        }

        ~DbContextFixture()
        {
            Dispose(false);
        }
    }

    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<DbContextFixture> { }
}
