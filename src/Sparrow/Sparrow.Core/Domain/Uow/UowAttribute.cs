using System;
using System.Transactions;

namespace Sparrow.Core.Domain.Uow
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class UowAttribute : Attribute
    {
        /// <summary>
        /// Scope option.
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }

        /// <summary>
        /// Is this UOW transactional?
        /// Uses default value if not supplied.
        /// </summary>
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// Timeout of UOW As milliseconds.
        /// Uses default value if not supplied.
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// If this UOW is transactional, this option indicated the isolation level of the transaction.
        /// Uses default value if not supplied.
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// Used to prevent starting a unit of work for the method.
        /// If there is already a started unit of work, this property is ignored.
        /// Default: false.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Creates a new UowAttribute object.
        /// </summary>
        public UowAttribute()
        {

        }

        /// <summary>
        /// Creates a new <see cref="UowAttribute"/> object.
        /// </summary>
        /// <param name="isTransactional">
        /// Is this unit of work will be transactional?
        /// </param>
        public UowAttribute(bool isTransactional)
        {
            IsTransactional = isTransactional;
        }

        /// <summary>
        /// Creates a new <see cref="UowAttribute"/> object.
        /// </summary>
        /// <param name="timeout">As milliseconds</param>
        public UowAttribute(int timeout)
        {
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Creates a new <see cref="UowAttribute"/> object.
        /// </summary>
        /// <param name="isTransactional">Is this unit of work will be transactional?</param>
        /// <param name="timeout">As milliseconds</param>
        public UowAttribute(bool isTransactional, int timeout)
        {
            IsTransactional = isTransactional;
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Creates a new <see cref="UowAttribute"/> object.
        /// <see cref="IsTransactional"/> is automatically set to true.
        /// </summary>
        /// <param name="isolationLevel">Transaction isolation level</param>
        public UowAttribute(IsolationLevel isolationLevel)
        {
            IsTransactional = true;
            IsolationLevel = isolationLevel;
        }

        /// <summary>
        /// Creates a new <see cref="UowAttribute"/> object.
        /// <see cref="IsTransactional"/> is automatically set to true.
        /// </summary>
        /// <param name="isolationLevel">Transaction isolation level</param>
        /// <param name="timeout">Transaction  timeout as milliseconds</param>
        public UowAttribute(IsolationLevel isolationLevel, int timeout)
        {
            IsTransactional = true;
            IsolationLevel = isolationLevel;
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Creates a new <see cref="UowAttribute"/> object.
        /// <see cref="IsTransactional"/> is automatically set to true.
        /// </summary>
        /// <param name="scope">Transaction scope</param>
        public UowAttribute(TransactionScopeOption scope)
        {
            IsTransactional = true;
            Scope = scope;
        }

        /// <summary>
        /// Creates a new <see cref="UowAttribute"/> object.
        /// </summary>
        /// <param name="scope">Transaction scope</param>
        /// <param name="isTransactional">
        /// Is this unit of work will be transactional?
        /// </param>
        public UowAttribute(TransactionScopeOption scope, bool isTransactional)
        {
            Scope = scope;
            IsTransactional = isTransactional;
        }

        /// <summary>
        /// Creates a new <see cref="UowAttribute"/> object.
        /// <see cref="IsTransactional"/> is automatically set to true.
        /// </summary>
        /// <param name="scope">Transaction scope</param>
        /// <param name="timeout">Transaction  timeout as milliseconds</param>
        public UowAttribute(TransactionScopeOption scope, int timeout)
        {
            IsTransactional = true;
            Scope = scope;
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        internal UowOptions CreateOptions()
        {
            return new UowOptions
            {
                IsTransactional = IsTransactional ?? false,
                IsolationLevel = IsolationLevel,
                Timeout = Timeout,
                Scope = Scope
            };
        }
    }
}
