using System;

namespace Sparrow.Uow
{
    public interface IActiveUow
    {
        event EventHandler OnCompleted;
        event EventHandler OnDisposed;
        event EventHandler<Exception> OnFailed;

        UowOptions Options { get; set; }

        void Begin(UowOptions options);

        void SaveChanges();
    }
}
