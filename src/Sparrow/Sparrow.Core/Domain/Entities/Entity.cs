using System;
using System.Collections.Generic;

namespace Sparrow.Core.Domain.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    /// <typeparam name="TPrimaryKey">唯一标识类型</typeparam>
    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
        where TPrimaryKey : IEquatable<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }

        public virtual bool IsTransient()
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default))
            {
                return true;
            }

            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }
    }

    /// <summary>
    /// 实体， 唯一标识类型为 int
    /// </summary>
    public class Entity : Entity<int>, IEntity
    {

    }
}
