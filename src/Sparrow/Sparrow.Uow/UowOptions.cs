using System;
using System.Transactions;

namespace Sparrow.Uow
{
    public class UowOptions
    {
        public TransactionScopeOption? Scope { get; set; }

        public TimeSpan? Timeout { get; set; }

        public bool IsTranscational { get; set; }
    }
}
