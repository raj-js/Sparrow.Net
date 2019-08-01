using System;
using System.Data;

namespace Sparrow.Data
{
    public abstract class TransactionWapperBase : ITransactionWapper
    {
        public string Id { get; } 

        public virtual IDbTransaction DbTransaction { get; protected set; }

        public virtual TransactionOptions Options { get; protected set; }

        public virtual bool IsCommitted { get; protected set; }

        public virtual bool Commitable { get; protected set; }

        public event EventHandler OnSucceed;
        public event EventHandler<Exception> OnFailed;

        public TransactionWapperBase(TransactionOptions options)
        {
            Options = options;

            Id = Guid.NewGuid().ToString("N");
            Commitable = options.IsTransactional;
            IsCommitted = false;
        }

        public virtual void Commit()
        {
            try
            {
                if (Commitable && !IsCommitted)
                {
                    DbTransaction.Commit();
                    IsCommitted = true;
                    OnSucceed?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception e)
            {
                IsCommitted = true;
                Rollback();
                OnFailed?.Invoke(this, e);
                throw;
            }
        }

        public void Rollback()
        {
            if (Commitable && IsCommitted)
            {
                DbTransaction.Rollback();
            }
        }

        public void Dispose()
        {
            DbTransaction?.Dispose();
        }
    }
}
