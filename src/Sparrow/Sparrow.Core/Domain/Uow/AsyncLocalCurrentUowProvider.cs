using System.Threading;

namespace Sparrow.Core.Domain.Uow
{
    public class AsyncLocalCurrentUowProvider : ICurrentUowProvider
    {
        public IUow Current
        {
            get => GetCurrentUow();
            set => SetCurrentUow(value);
        }

        private static readonly AsyncLocal<LocalUowWrapper> AsyncLocalUow = new AsyncLocal<LocalUowWrapper>();

        private static IUow GetCurrentUow()
        {
            lock (AsyncLocalUow)
            {
                var uow = AsyncLocalUow.Value?.Uow;
                if (uow == null)
                {
                    return null;
                }

                if (uow.IsDisposed)
                {
                    AsyncLocalUow.Value = null;
                    return null;
                }

                return uow;
            }
        }

        private static void SetCurrentUow(IUow value)
        {
            lock (AsyncLocalUow)
            {
                if (value == null)
                {
                    if (AsyncLocalUow.Value == null)
                    {
                        return;
                    }

                    if (AsyncLocalUow.Value.Uow?.Outer == null)
                    {
                        AsyncLocalUow.Value.Uow = null;
                        AsyncLocalUow.Value = null;
                        return;
                    }

                    AsyncLocalUow.Value.Uow = AsyncLocalUow.Value.Uow.Outer;
                }
                else
                {
                    if (AsyncLocalUow.Value?.Uow == null)
                    {
                        if (AsyncLocalUow.Value != null)
                        {
                            AsyncLocalUow.Value.Uow = value;
                        }

                        AsyncLocalUow.Value = new LocalUowWrapper(value);
                        return;
                    }

                    value.Outer = AsyncLocalUow.Value.Uow;
                    AsyncLocalUow.Value.Uow = value;
                }
            }
        }

        private class LocalUowWrapper
        {
            public IUow Uow { get; set; }

            public LocalUowWrapper(IUow uow)
            {
                Uow = uow;
            }
        }
    }
}
