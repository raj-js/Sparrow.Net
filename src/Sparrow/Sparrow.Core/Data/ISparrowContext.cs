using System;

namespace Sparrow.Data
{
    /// <summary>
    /// Sparrow 数据上下文
    /// 
    /// Scope 对象
    /// </summary>
    public interface ISparrowContext : IDisposable
    {
        IConnectionWapper GetOrCreate(string connectionString);

        ITransactionWapper BeginTransaction(IConnectionWapper connectionWapper);

        void Commit();

        void Rollback();
    }
}
