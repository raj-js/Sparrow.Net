using System.Transactions;

namespace Sparrow.Core.Domain.Uow
{
    /// <summary>
    /// 工作单元管理器
    /// </summary>
    public interface IUowManager
    {
        /// <summary>
        /// 当前工作单元
        /// </summary>
        IActiveUow Current { get; }

        /// <summary>
        /// 启动工作单元
        /// </summary>
        /// <returns></returns>
        IUowCompleteHandle Begin();

        /// <summary>
        /// 启动工作单元
        /// </summary>
        /// <param name="scopeOption">事务选项</param>
        /// <returns></returns>
        IUowCompleteHandle Begin(TransactionScopeOption scopeOption);

        /// <summary>
        /// 启动工作单元
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IUowCompleteHandle Begin(UowArgs args);
    }
}
