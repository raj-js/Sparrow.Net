using System;
using System.Data;

namespace Sparrow.Data
{
    public abstract class ConnectionWapperBase : IConnectionWapper
    {
        protected string ConnectionString { get; set; }

        public IDbConnection DbConnection { get; protected set; }

        public bool HasTransaction { get; set; }

        public virtual ITransactionWapper Transaction { get; protected set; }

        public abstract ITransactionWapper BeginTransaction(TransactionOptions options);

        public virtual void Dispose()
        {
            if (HasTransaction)
            {
                Transaction?.Dispose();
            }
        }
    }
}
