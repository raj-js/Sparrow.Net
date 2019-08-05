using System;
using System.Data;

namespace Sparrow.Data
{
    public interface ITransactionWapper : IDisposable
    {
        string Id { get; }

        event EventHandler OnSucceed;
        event EventHandler<Exception> OnFailed;

        IDbTransaction DbTransaction { get; set; }

        TransactionOptions Options { get; }

        bool IsCommitted { get; }

        bool Commitable { get; }

        void Commit();

        void Rollback();
    }
}
