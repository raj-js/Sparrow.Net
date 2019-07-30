using System;
using System.Transactions;
using Sparrow.Core.Dependency;

namespace Sparrow.Core.Domain.Uow
{
    internal class UowManager : IUowManager
    {
        private readonly IIocResolver _iocResolver;
        private readonly ICurrentUowProvider _currentUowProvider;

        public IActiveUow Current => _currentUowProvider.Current;

        public UowManager(
            IIocResolver iocResolver,
            ICurrentUowProvider currentUowProvider
            )
        {
            _iocResolver = iocResolver;
            _currentUowProvider = currentUowProvider;
        }

        public IUowCompleteHandle Begin()
        {
            throw new NotImplementedException();
        }

        public IUowCompleteHandle Begin(TransactionScopeOption scopeOption)
        {
            throw new NotImplementedException();
        }

        public IUowCompleteHandle Begin(UowArgs args)
        {
            var outerUow = _currentUowProvider.Current;

            if (args.ScopeOption == TransactionScopeOption.Required && outerUow != null)
                return new InnerUowCompleteHandle();

            var uow = _iocResolver.Resolve<IUow>();

            uow.Completed += (s, _) => { _currentUowProvider.Current = null; };
            uow.Failed += (s, e) => { _currentUowProvider.Current = null; };
            uow.Disposed += (s, e) => { _iocResolver.Release(uow); };

            _currentUowProvider.Current = uow;

            return uow;
        }
    }
}
