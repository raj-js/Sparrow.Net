using System;
using System.Transactions;

namespace Sparrow.Core.Domain.Uow
{
    public class UowArgs
    {
        /// <summary>
        /// 事务超时时间
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// 事务选项
        /// </summary>
        public TransactionScopeOption? ScopeOption { get; set; }

        /// <summary>
        /// 是否为事务操作
        /// </summary>
        public bool? IsTransactional { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }
    }
}
