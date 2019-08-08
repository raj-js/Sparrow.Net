using Castle.DynamicProxy;
using Sparrow.Core.Threading;
using System.Reflection;
using System.Threading.Tasks;

namespace Sparrow.Core.Domain.Uow
{
    internal class UowInterceptor : IInterceptor
    {
        private readonly IUowManager _uowManager;
        private readonly UowOptions _uowUowOptions;

        public UowInterceptor(IUowManager uowManager, UowOptions uowOptions)
        {
            _uowManager = uowManager;
            _uowUowOptions = uowOptions;
        }

        public void Intercept(IInvocation invocation)
        {
            MethodInfo method;
            try
            {
                method = invocation.MethodInvocationTarget;
            }
            catch
            {
                method = invocation.GetConcreteMethod();
            }

            var uowAttr = _uowUowOptions.GetUnitOfWorkAttributeOrNull(method);
            if (uowAttr == null || uowAttr.IsDisabled)
            {
                invocation.Proceed();
                return;
            }

            if (invocation.Method.IsAsync())
            {
                PerformAsyncUow(invocation, uowAttr.CreateOptions());
            }
            else
            {
                PerformSyncUow(invocation, uowAttr.CreateOptions());
            }
        }

        private void PerformSyncUow(IInvocation invocation, UowOptions options)
        {
            using (var uow = _uowManager.Begin(options))
            {
                invocation.Proceed();
                uow.Complete();
            }
        }

        private void PerformAsyncUow(IInvocation invocation, UowOptions options)
        {
            var uow = _uowManager.Begin(options);
            try
            {
                invocation.Proceed();
            }
            catch
            {
                uow.Dispose();
                throw;
            }

            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                    (Task)invocation.ReturnValue,
                    async () => await uow.CompleteAsync(),
                    exception => uow.Dispose()
                );
            }
            else //Task<TResult>
            {
                invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                    invocation.Method.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                    async () => await uow.CompleteAsync(),
                    exception => uow.Dispose()
                );
            }
        }
    }
}
