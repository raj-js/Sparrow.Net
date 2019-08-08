namespace Sparrow.Core.Domain.Uow
{
    public interface IUow : IActiveUow, IUowHandle
    {
        string Id { get; }

        bool IsDisposed { get; }
    }
}
 