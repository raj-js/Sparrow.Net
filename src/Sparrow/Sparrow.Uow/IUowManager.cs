using System;
using System.Transactions;

namespace Sparrow.Uow
{
    public interface IUowManager
    {
        IUow Current { get; }

        IUowHandle Begin();

        IUowHandle Begin(TransactionScopeOption scope);

        IUowHandle Begin(UowOptions options);
    }
}
