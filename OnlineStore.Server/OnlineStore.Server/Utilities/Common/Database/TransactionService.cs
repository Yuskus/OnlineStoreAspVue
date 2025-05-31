using Microsoft.EntityFrameworkCore.Storage;
using OnlineStore.Server.Database.Context;

namespace OnlineStore.Server.Utilities.Common.Database;

public class TransactionService(OnlineStoreDbContext context) : ITransactionService
{
    private readonly OnlineStoreDbContext _context = context;
    private IDbContextTransaction? _transaction;
    private bool disposed = false;

    public void BeginTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Commit()
    {
        _transaction?.Commit();
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
        }
    }

    public void Rollback()
    {
        _transaction?.Rollback();
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            disposed = true;
            await _transaction.DisposeAsync();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed) return;
        if (disposing && _transaction != null)
        {
            _transaction.Dispose();
        }
        disposed = true;
    }

    ~TransactionService()
    {
        Dispose(false);
    }
}
