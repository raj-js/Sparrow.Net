using System;

namespace Sparrow.Core.Domain.Entities
{
    /// <summary>
    /// 实体, 唯一标识类型为 int
    /// </summary>
    public interface IEntity : IEntity<int>
    {

    }

    /// <summary>
    /// 实体
    /// </summary>
    /// <typeparam name="TPrimaryKey">唯一标识类型</typeparam>
    public interface IEntity<TPrimaryKey>
        where TPrimaryKey : IEquatable<TPrimaryKey>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        TPrimaryKey Id { get; set; }

        /// <summary>
        /// 是否为瞬态
        /// </summary>
        /// <returns></returns>
        bool IsTransient();
    }
}
