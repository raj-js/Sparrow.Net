using System;

namespace Sparrow.Core.Domain.Uow
{
    /// <summary>
    /// 工作单元完成处理
    /// </summary>
    public interface IUowCompleteHandle : IDisposable
    {
        /// <summary>
        /// 完成工作单元
        /// </summary>
        void Complete();
    }
}
