using System.Transactions;

namespace Sparrow.Core.Domain.Uow
{
    public interface IUowManager
    {
        IUow Current { get; }

        IUowHandle Begin();

        IUowHandle Begin(TransactionScopeOption scope);

        IUowHandle Begin(UowOptions options);
    }
}
 