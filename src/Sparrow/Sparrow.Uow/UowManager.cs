using System;
using System.Transactions;

namespace Sparrow.Uow
{
    public class UowManager : IUowManager
    {
        public IUow Current { get; protected set; }

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
            var outerUow = Current;

            // 使用范围事务
            if (outerUow != null && options.Scope == TransactionScopeOption.Required)
                return new InnerUowHandle();

            // 使用 Ioc 完成解析
            IUow uow = null;

            uow.OnCompleted += (_, __) => Current = null;
            uow.OnFailed += (_, __) => Current = null;
            uow.OnDisposed += (_, __) => { };

            Current = uow;

            return uow;
        }
    }
}
