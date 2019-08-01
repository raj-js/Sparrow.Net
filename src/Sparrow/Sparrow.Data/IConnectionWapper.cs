using System;
using System.Data;

namespace Sparrow.Data
{
    public interface IConnectionWapper : IDisposable
    {
        bool HasTransaction { get; }

        IDbConnection DbConnection { get; }

        ITransactionWapper Transaction { get; }

        ITransactionWapper BeginTransaction(TransactionOptions options);
    }
}
