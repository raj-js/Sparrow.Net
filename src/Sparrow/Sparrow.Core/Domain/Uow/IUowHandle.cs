using System;
using System.Threading.Tasks;

namespace Sparrow.Core.Domain.Uow
{
    public interface IUowHandle : IDisposable
    {
        void Complete();

        Task CompleteAsync();
    }
}
 