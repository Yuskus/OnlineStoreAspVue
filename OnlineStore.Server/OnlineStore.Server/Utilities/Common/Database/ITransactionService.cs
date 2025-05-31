namespace OnlineStore.Server.Utilities.Common.Database;

public interface ITransactionService : IDisposable
{
    void BeginTransaction();
    Task BeginTransactionAsync();
    int SaveChanges();
    Task<int> SaveChangesAsync();
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
    ValueTask DisposeAsync();
}
