using System.Transactions;

namespace Sparrow.Core.Domain.Uow
{
    public class UowArgs
    {
        public int Timeout { get; set; }

        public TransactionScopeOption ScopeOption { get; set; }
    }
}
