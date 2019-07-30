using System;

namespace Sparrow.Core.Domain.Uow
{
    /// <summary>
    /// 活动的工作单元
    /// </summary>
    public interface IActiveUow
    {
        event EventHandler Completed;
        event EventHandler<Exception> Failed;
        event EventHandler Disposed;

        /// <summary>
        /// 工作单元启动参数
        /// </summary>
        UowArgs Args { get; set; }

        /// <summary>
        /// 是否释放
        /// </summary>
        bool IsDisposed { get; set; }

        /// <summary>
        /// 保存更改
        /// </summary>
        void SaveChanges();
    }
}
