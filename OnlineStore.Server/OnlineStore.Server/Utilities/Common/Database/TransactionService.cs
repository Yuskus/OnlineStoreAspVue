using Microsoft.EntityFrameworkCore.Storage;
using OnlineStore.Server.Database.Context;

namespace OnlineStore.Server.Utilities.Common.Database;

public class TransactionService(OnlineStoreDbContext dbContext) : ITransactionService
{
    private readonly OnlineStoreDbContext _dbContext = dbContext;

    public IDbContextTransaction BeginTransaction()
    {
        return _dbContext.Database.BeginTransaction();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _dbContext.Database.BeginTransactionAsync();
    }
}
