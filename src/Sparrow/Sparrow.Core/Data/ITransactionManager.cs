using System;
using System.Data;

namespace Sparrow.Core.Data
{
    public interface ITransactionManager : IDisposable
    {
        void Begin();

        void Complete();
    }
}
