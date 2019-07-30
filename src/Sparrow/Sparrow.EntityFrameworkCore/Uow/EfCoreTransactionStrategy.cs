using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Sparrow.Core.Dependency;
using Sparrow.Core.Domain.Uow;
using Sparrow.Core.Transactions.Extensions;
using System.Collections.Generic;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Sparrow.EntityFrameworkCore.Uow
{
    public class EfCoreTransactionStrategy: IEfCoreTransactionStrategy
    {
        protected UowArgs Args { get; private set; }
        protected IDictionary<string, ActiveTransactionInfo> ActiveTransactions { get; }

        public EfCoreTransactionStrategy()
        {
            ActiveTransactions = new Dictionary<string, ActiveTransactionInfo>();
        }

        public void InitArgs(UowArgs args)
        {
            Args = args;
        }

        public DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver) where TDbContext : DbContext
        {
            DbContext dbContext;

            ActiveTransactions.TryGetValue(connectionString, out var activeTransaction);
            if (activeTransaction == null)
            {
                dbContext = dbContextResolver.Resolve<TDbContext>(connectionString, null);

                var dbTransaction = dbContext.Database.BeginTransaction((Args.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());
                activeTransaction = new ActiveTransactionInfo(dbTransaction, dbContext);
                ActiveTransactions[connectionString] = activeTransaction;
            }
            else
            {
                dbContext = dbContextResolver.Resolve<TDbContext>(
                    connectionString,
                    activeTransaction.DbContextTransaction.GetDbTransaction().Connection
                );

                if (dbContext.HasRelationalTransactionManager())
                {
                    dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.GetDbTransaction());
                }
                else
                {
                    dbContext.Database.BeginTransaction();
                }

                activeTransaction.AttendedDbContexts.Add(dbContext);
            }

            return dbContext;
        }

        public void Commit()
        {
            foreach (var activeTransaction in ActiveTransactions.Values)
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

        public void Dispose(IIocResolver iocResolver)
        {
            foreach (var activeTransaction in ActiveTransactions.Values)
            {
                activeTransaction.DbContextTransaction.Dispose();

                foreach (var attendedDbContext in activeTransaction.AttendedDbContexts)
                {
                    iocResolver.Release(attendedDbContext);
                }

                iocResolver.Release(activeTransaction.StarterDbContext);
            }

            ActiveTransactions.Clear();
        }
    }
}
