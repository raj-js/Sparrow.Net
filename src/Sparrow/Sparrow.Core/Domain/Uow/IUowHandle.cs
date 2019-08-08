using System;

namespace Sparrow.Core.Domain.Uow
{
    public interface IUowHandle : IDisposable
    {
        void Complete();
    }
}
 