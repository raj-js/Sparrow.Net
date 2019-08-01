using System;

namespace Sparrow.Uow
{
    public interface IUowHandle : IDisposable
    {
        void Complete();
    }
}
