using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Data;
using Sparrow.Core.Dependency;
using Sparrow.Uow;
using System.Collections.Generic;

namespace Sparrow.EntityFrameworkCore.Uow
{
    public class EfCoreUow : UowBase, ITransientDependency
    {
        private IDictionary<string, DbContext> _activeDbContexts;
        private IIocResolver _iocResolver;
        private readonly IDbContextResolver _dbContextResolver;
        private readonly IEfCoreTransactionStrategy _efCoreTransactionStrategy;

        public EfCoreUow(IIocResolver iocResolver,
            IConnectionStringResolver connectionStringResolver,
            IDbContextResolver dbContextResolver,
            IEfCoreTransactionStrategy efCoreTransactionStrategy)
            : base(connectionStringResolver)
        {
            _iocResolver = iocResolver;
            _dbContextResolver = dbContextResolver;
            _efCoreTransactionStrategy = efCoreTransactionStrategy;

            _activeDbContexts = new Dictionary<string, DbContext>();
        }

        protected override void BeginUow()
        {
            if (Options.IsTransactional)
                _efCoreTransactionStrategy.InitOptions(Options);
        }

        protected override void CompleteUow()
        {
            foreach (var context in _activeDbContexts.Values)
            {
                context.SaveChanges();
            }

            if (Options.IsTransactional)
                _efCoreTransactionStrategy.Commit();
        }

        protected override void DisposeUow()
        {
            if (Options.IsTransactional)
                _efCoreTransactionStrategy.Dispose();
            else
            {
                foreach (var context in _activeDbContexts.Values)
                {
                    context.Dispose();
                    _iocResolver.Release(context);
                }
            }
           _activeDbContexts.Clear();
        }

        public TDbContext GetDbContext<TDbContext>() where TDbContext : DbContext
        {
            var connectionString = ResolveConnectionString();
            var dbContextKey = $"{typeof(TDbContext).FullName}#{connectionString}";

            if (!_activeDbContexts.TryGetValue(dbContextKey, out var dbContext))
            {
                if (Options.IsTransactional)
                    dbContext = _efCoreTransactionStrategy.CreateDbContext<TDbContext>(connectionString);
                else
                    dbContext = _dbContextResolver.Resolve<TDbContext>(connectionString, null);

                _activeDbContexts.Add(dbContextKey, dbContext);
            }
            return (TDbContext) dbContext;
        }
    }
}
