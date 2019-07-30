using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Dependency;
using Sparrow.Core.Domain.Uow;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sparrow.EntityFrameworkCore.Uow
{
    public class EfCoreUow : UowBase
    {
        private readonly IDbContextResolver _dbContextResolver;
        // private readonly IDbContextTypeMatcher _dbContextTypeMatcher;
        private readonly IEfCoreTransactionStrategy _transactionStrategy;

        protected IDictionary<string, DbContext> ActiveDbContexts { get; }
        protected IIocResolver IocResolver { get; }

        public EfCoreUow(
            IIocResolver iocResolver, 
            IDbContextResolver dbContextResolver, 
            IEfCoreTransactionStrategy transactionStrategy
            )
        {
            IocResolver = iocResolver;
            _dbContextResolver = dbContextResolver;
            _transactionStrategy = transactionStrategy;
            ActiveDbContexts = new Dictionary<string, DbContext>();
        }

        protected override void BeginUow()
        {
            if (Args.IsTransactional == true)
            {
                _transactionStrategy.InitArgs(Args);
            }
        }

        public override void SaveChanges()
        {
            foreach (var dbContext in GetAllActiveDbContexts())
            {
                dbContext.SaveChanges();
            }
        }

        protected override void CompleteUow()
        {
            SaveChanges();
            CommitTransaction();
        }

        protected override void DisposeUow()
        {
            if (Args.IsTransactional == true)
            {
                _transactionStrategy.Dispose(IocResolver);
            }
            else
            {
                foreach (var context in GetAllActiveDbContexts())
                {
                    Release(context);
                }
            }

            ActiveDbContexts.Clear();
        }

        protected virtual void Release(DbContext dbContext)
        {
            dbContext.Dispose();
            IocResolver.Release(dbContext);
        }

        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            return null;
        }

        public IReadOnlyList<DbContext> GetAllActiveDbContexts()
        {
            return ActiveDbContexts.Values.ToImmutableList();
        }

        private void CommitTransaction()
        {
            if (Args.IsTransactional == true)
            {
                _transactionStrategy.Commit();
            }
        }
    }
}
