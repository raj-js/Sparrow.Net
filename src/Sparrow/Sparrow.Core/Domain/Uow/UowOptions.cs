using System;
using System.Collections.Generic;
using System.Transactions;
using Sparrow.Core.Domain.Repositories;

namespace Sparrow.Core.Domain.Uow
{
    public class UowOptions
    {
        public TransactionScopeOption? Scope { get; set; }

        public TimeSpan? Timeout { get; set; }

        public bool? IsTransactional { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public List<Func<Type, bool>> ConventionalUowSelectors { get; }

        public UowOptions()
        {
            Scope = TransactionScopeOption.Required;
            Timeout = TimeSpan.FromSeconds(300);
            IsTransactional = true;
            ConventionalUowSelectors = new List<Func<Type, bool>>()
            {
                type => typeof(IRepository).IsAssignableFrom(type)
            };
        }

        public static UowOptions Default = new UowOptions();
    }
}
