using Sparrow.Core.Data;
using System;

namespace Sparrow.Uow
{
    public abstract class UowBase : IUow
    {
        public string Id { get; }
        public UowOptions Options { get; protected set; }
        public bool IsDisposed { get; protected set; }
        public IUow Outer { get; set; }

        public event EventHandler OnCompleted;
        public event EventHandler OnDisposed;
        public event EventHandler<Exception> OnFailed;

        private bool _isBeginCalledBefore;
        private bool _isCompleteCalledBefore;

        protected IConnectionStringResolver ConnectionStringResolver;

        public UowBase(IConnectionStringResolver connectionStringResolver)
        {
            Id = Guid.NewGuid().ToString("N");

            ConnectionStringResolver = connectionStringResolver;
        }

        public void Begin(UowOptions options)
        {
            Options = options;

            if (_isBeginCalledBefore)
                throw new Exception("不可以多次调用 Begin 方法");

            _isBeginCalledBefore = true;

            BeginUow();
        }

        public void Complete()
        {
            if (_isCompleteCalledBefore)
                throw new Exception("不可以多次调用 Begin 方法");

            _isCompleteCalledBefore = true;

            try
            {
                CompleteUow();
                OnCompleted?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                OnFailed?.Invoke(this, e);
                throw;
            }
        }

        public void Dispose()
        {
            DisposeUow();

            IsDisposed = true;
        }

        protected abstract void BeginUow();

        protected abstract void CompleteUow();

        protected abstract void DisposeUow();

        protected string ResolveConnectionString()
        {
            return ConnectionStringResolver.GetConnectionString(new ConnectionStringResolveArgs());
        }

        protected string ResolveConnectionString(ConnectionStringResolveArgs args)
        {
            return ConnectionStringResolver.GetConnectionString(args);
        }
    }
}
