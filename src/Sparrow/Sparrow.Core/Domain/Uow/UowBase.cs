using Castle.Core;
using System;

namespace Sparrow.Core.Domain.Uow
{
    /// <summary>
    /// 工作单元基类
    /// </summary>
    public abstract class UowBase : IUow
    {
        private bool _isCompleteCalledBefore;
        private bool _isBeginCalledBefore;
        private bool _succeed;
        private Exception _exception;

        protected IConnectionStringResolver ConnectionStringResolver;

        protected UowBase(IConnectionStringResolver connectionStringResolver)
        {
            ConnectionStringResolver = connectionStringResolver;
        }

        public string Id { get; set; }

        [DoNotWire]
        public IUow Outer { get; set; }

        public event EventHandler Completed;
        public event EventHandler<Exception> Failed;
        public event EventHandler Disposed;

        public UowArgs Args { get; set; }

        public bool IsDisposed { get; set; }

        public abstract void SaveChanges();

        protected virtual void BeginUow()
        {

        }

        protected abstract void CompleteUow();

        protected abstract void DisposeUow();

        protected virtual void OnCompleted()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnFailed(Exception e)
        {
            Failed?.Invoke(this, e);
        }

        protected virtual void OnDisposed()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public void Begin(UowArgs args)
        {
            if (_isBeginCalledBefore)
                throw new Exception("已经调用过 Begin 方法");

            _isBeginCalledBefore = true;

            Args = args;

            BeginUow();
        }

        public void Complete()
        {
            if (_isCompleteCalledBefore)
                throw new Exception("已经调用过 Complete 方法");

            _isCompleteCalledBefore = true;

            try
            {
                CompleteUow();
                _succeed = true;
                OnCompleted();
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
                OnFailed(_exception);
            }

            DisposeUow();
            OnDisposed();
        }

        protected virtual string ResolveConnectionString(ConnectionStringResolveArgs args)
        {
            return ConnectionStringResolver.GetNameOrConnectionString(args);
        }
    }
}
