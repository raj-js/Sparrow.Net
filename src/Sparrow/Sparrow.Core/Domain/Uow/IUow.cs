namespace Sparrow.Core.Domain.Uow
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUow : IActiveUow, IUowCompleteHandle
    {
        /// <summary>
        /// 工作单元唯一标识
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 外层工作单元
        /// </summary>
        IUow Outer { get; set; }

        /// <summary>
        /// 启动工作单元
        /// </summary>
        /// <param name="args">启动参数</param>
        void Begin(UowArgs args);
    }
}
