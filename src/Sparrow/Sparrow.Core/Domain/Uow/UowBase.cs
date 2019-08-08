using System;
using System.Threading.Tasks;
using Castle.Core;
using Sparrow.Core.Data;

namespace Sparrow.Core.Domain.Uow
{
    public abstract class UowBase : IUow
    {
        public string Id { get; }
        public UowOptions Options { get; protected set; }
        public bool IsDisposed { get; protected set; }

        [DoNotWire]
        public IUow Outer { get; set; }

        public event EventHandler OnCompleted;
        public event EventHandler OnDisposed;
        public event EventHandler<Exception> OnFailed;

        private bool _isBeginCalledBefore;
        private bool _isCompleteCalledBefore;
        private Exception _exception;
        private bool _succeed;

        protected IConnectionStringResolver ConnectionStringResolver;

        protected UowBase(IConnectionStringResolver connectionStringResolver)
        {
            Id = Guid.NewGuid().ToString("N");

            ConnectionStringResolver = connectionStringResolver;
        }

        public void Begin(UowOptions options)
        {

            if (_isBeginCalledBefore)
                throw new Exception("不可以多次调用 Begin 方法");

            _isBeginCalledBefore = true;

            Options = options;

            BeginUow();
        }

        public void Complete()
        {
            PreventMultipleComplete();

            try
            {
                CompleteUow();
                _succeed = true;
                OnCompleted?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                _exception = e;
                _succeed = false;
                throw;
            }
        }

        public async Task CompleteAsync()
        {
            PreventMultipleComplete();

            try
            {
                await CompleteUowAsync();
                _succeed = true;
                OnCompleted?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        public void Dispose()
        {
            if (!_isBeginCalledBefore || IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (!_succeed)
            {
                OnFailed?.Invoke(this, _exception);
            }

            DisposeUow();
            OnDisposed?.Invoke(this, EventArgs.Empty);
        }

        protected abstract void BeginUow();

        protected abstract void CompleteUow();

        protected abstract Task CompleteUowAsync();

        protected abstract void DisposeUow();

        protected string ResolveConnectionString()
        {
            return ConnectionStringResolver.GetConnectionString(new ConnectionStringResolveArgs());
        }

        protected string ResolveConnectionString(ConnectionStringResolveArgs args)
        {
            return ConnectionStringResolver.GetConnectionString(args);
        }

        private void PreventMultipleComplete()
        {
            if (_isCompleteCalledBefore)
                throw new Exception("不可以多次调用 Begin 方法");

            _isCompleteCalledBefore = true;
        }
    }
}
