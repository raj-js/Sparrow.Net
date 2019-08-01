using System;

namespace Sparrow.Uow
{
    public interface IActiveUow
    {
        event EventHandler OnCompleted;
        event EventHandler OnDisposed;
        event EventHandler<Exception> OnFailed;

        IUow Outer { get; set; }

        UowOptions Options { get; }

        void Begin(UowOptions options);
    }
}
