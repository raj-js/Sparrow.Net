using System.Transactions;
using Sparrow.Core.Dependency;

namespace Sparrow.Core.Domain.Uow
{
    public class UowManager : IUowManager, ITransientDependency
    {
        private readonly IIocResolver _iocResolver;
        private readonly ICurrentUowProvider _currentUowProvider;

        public IUow Current => _currentUowProvider.Current;

        public UowManager(IIocResolver iocResolver, ICurrentUowProvider currentUowProvider)
        {
            _iocResolver = iocResolver;
            _currentUowProvider = currentUowProvider;
        }

        public IUowHandle Begin()
        {
            return Begin(new UowOptions());
        }

        public IUowHandle Begin(TransactionScopeOption scope)
        {
            return Begin(new UowOptions { Scope = scope });
        }

        public IUowHandle Begin(UowOptions options)
        {
            var outerUow = _currentUowProvider.Current;

            if (outerUow != null && options.Scope == TransactionScopeOption.Required)
                return new InnerUowHandle();

            var uow = _iocResolver.Resolve<IUow>();

            uow.OnCompleted += (_, __) => _currentUowProvider.Current = null;
            uow.OnFailed += (_, __) => _currentUowProvider.Current = null;
            uow.OnDisposed += (_, __) => _iocResolver.Release(uow);

            uow.Begin(options);

            _currentUowProvider.Current = uow;

            return uow;
        }
    }
}
