using System;

namespace Sparrow.Uow
{
    public interface IUowHandle : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
