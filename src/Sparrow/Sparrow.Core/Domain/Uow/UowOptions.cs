using System;
using System.Transactions;

namespace Sparrow.Core.Domain.Uow
{
    public class UowOptions
    {
        public TransactionScopeOption? Scope { get; set; }

        public TimeSpan? Timeout { get; set; }

        public bool IsTransactional { get; set; }

        public UowOptions()
        {
            Scope = TransactionScopeOption.Required;
            Timeout = TimeSpan.FromSeconds(300);
            IsTransactional = true;
        }
    }
}
