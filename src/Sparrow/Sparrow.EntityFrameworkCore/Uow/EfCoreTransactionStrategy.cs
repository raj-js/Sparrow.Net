using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Sparrow.Core.Dependency;
using Sparrow.Uow;
using System.Collections.Generic;

namespace Sparrow.EntityFrameworkCore.Uow
{
    public class EfCoreTransactionStrategy : IEfCoreTransactionStrategy
    {
        private UowOptions _options;
        private readonly IDictionary<string, ActiveTransactionInfo> _activeTransactions;
        private readonly IIocResolver _iocResolver;
        private readonly IDbContextResolver _dbContextResolver;

        public EfCoreTransactionStrategy(IIocResolver iocResolver, IDbContextResolver dbContextResolver)
        {
            _activeTransactions = new Dictionary<string, ActiveTransactionInfo>();

            _iocResolver = iocResolver;
            _dbContextResolver = dbContextResolver;
        }

        public void InitOptions(UowOptions options)
        {
            _options = options;
        }

        public DbContext CreateDbContext<TDbContext>(string connectionString) where TDbContext : DbContext
        {
            DbContext dbContext;

            if (!_activeTransactions.TryGetValue(connectionString, out var transactionInfo))
            {
                dbContext = _dbContextResolver.Resolve<TDbContext>(connectionString, null);
                var transaction = dbContext.Database.BeginTransaction();
                transactionInfo = new ActiveTransactionInfo(transaction, dbContext);
                _activeTransactions[connectionString] = transactionInfo;
            }
            else
            {
                dbContext = _dbContextResolver.Resolve<TDbContext>(
                    connectionString,
                    transactionInfo.DbContextTransaction.GetDbTransaction().Connection
                );

                if (dbContext.HasRelationalTransactionManager())
                    dbContext.Database.UseTransaction(transactionInfo.DbContextTransaction.GetDbTransaction());
                else
                    dbContext.Database.BeginTransaction();

                transactionInfo.AttendedDbContexts.Add(dbContext);
            }
            return dbContext;
        }

        public void Commit()
        {
            foreach (var activeTransaction in _activeTransactions.Values)
            {
                activeTransaction.DbContextTransaction.Commit();

                foreach (var dbContext in activeTransaction.AttendedDbContexts)
                {
                    if (dbContext.HasRelationalTransactionManager())
                    {
                        continue; //Relational databases use the shared transaction
                    }

                    dbContext.Database.CommitTransaction();
                }
            }
        }

        public void Dispose()
        {
            foreach (var activeTransaction in _activeTransactions.Values)
            {
                activeTransaction.DbContextTransaction.Dispose();

                foreach (var attendedDbContext in activeTransaction.AttendedDbContexts)
                {
                    _iocResolver.Release(attendedDbContext);
                }

                _iocResolver.Release(activeTransaction.StarterDbContext);
            }

            _activeTransactions.Clear();
        }
    }
}
