namespace Sparrow.Uow
{
    public interface IUow : IActiveUow, IUowHandle
    {
        string Id { get; }
    }
}
