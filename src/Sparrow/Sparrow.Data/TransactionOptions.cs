using System.Data;

namespace Sparrow.Data
{
    public class TransactionOptions
    {
        public bool IsTransactional { get; set; }

        public IsolationLevel IsolationLevel { get; set; }

        public static TransactionOptions NoTransaction => new TransactionOptions
        {
            IsTransactional = false
        };
    }
}
