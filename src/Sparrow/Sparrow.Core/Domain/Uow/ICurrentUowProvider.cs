using Sparrow.Uow;

namespace Sparrow.Core.Domain.Uow
{
    public interface ICurrentUowProvider
    {
        IUow Current { get; set; }
    }
}
