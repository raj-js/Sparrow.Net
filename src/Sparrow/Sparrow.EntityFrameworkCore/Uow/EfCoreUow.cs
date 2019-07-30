using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Dependency;
using Sparrow.Core.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sparrow.EntityFrameworkCore.Uow
{
    public class EfCoreUow : UowBase
    {
        private readonly IDbContextResolver _dbContextResolver;
        private readonly IDbContextTypeMatcher _dbContextTypeMatcher;
        private readonly IEfCoreTransactionStrategy _transactionStrategy;

        protected IDictionary<string, DbContext> ActiveDbContexts { get; }
        protected IIocResolver IocResolver { get; }

        public EfCoreUow(
            IIocResolver iocResolver,
            IDbContextResolver dbContextResolver,
            IEfCoreTransactionStrategy transactionStrategy,
            IConnectionStringResolver connectionStringResolver, 
            IDbContextTypeMatcher dbContextTypeMatcher)
            : base(connectionStringResolver)
        {
            IocResolver = iocResolver;
            _dbContextResolver = dbContextResolver;
            _transactionStrategy = transactionStrategy;
            _dbContextTypeMatcher = dbContextTypeMatcher;
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

        public virtual TDbContext GetOrCreateDbContext<TDbContext>(string name = null)
            where TDbContext : DbContext
        {
            var concreteDbContextType = _dbContextTypeMatcher.GetConcreteType(typeof(TDbContext));

            var connectionStringResolveArgs = new ConnectionStringResolveArgs
            {
                ["DbContextType"] = typeof(TDbContext),
                ["DbContextConcreteType"] = concreteDbContextType
            };
            var connectionString = ResolveConnectionString(connectionStringResolveArgs);

            var dbContextKey = concreteDbContextType.FullName + "#" + connectionString;
            if (name != null)
            {
                dbContextKey += "#" + name;
            }

            if (ActiveDbContexts.TryGetValue(dbContextKey, out var dbContext)) return (TDbContext) dbContext;

            dbContext = Args.IsTransactional == true ? 
                _transactionStrategy.CreateDbContext<TDbContext>(connectionString, _dbContextResolver) : 
                _dbContextResolver.Resolve<TDbContext>(connectionString, null);

            if (Args.Timeout.HasValue &&
                dbContext.Database.IsRelational() &&
                !dbContext.Database.GetCommandTimeout().HasValue)
            {
                dbContext.Database.SetCommandTimeout(Convert.ToInt32(Args.Timeout.Value.TotalSeconds));
            }
            ActiveDbContexts[dbContextKey] = dbContext;

            return (TDbContext)dbContext;
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
