using Microsoft.EntityFrameworkCore.Storage;

namespace OnlineStore.Server.Utilities.Common.Database;

public interface ITransactionService
{
    IDbContextTransaction BeginTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync();
}
