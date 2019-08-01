using System;
using System.Collections.Generic;

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

        void Commit();

        void Rollback();
    }
}
