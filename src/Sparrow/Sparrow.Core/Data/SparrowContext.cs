using Sparrow.Core.Data;
using System;
using System.Collections.Generic;

namespace Sparrow.Data
{
    public class SparrowContext : ISparrowContext
    {
        private readonly IConnectionFactory _factory;

        protected Dictionary<string, IConnectionWapper> Connections;

        public SparrowContext(IConnectionFactory factory)
        {
            Connections = new Dictionary<string, IConnectionWapper>();
            _factory = factory;
        }

        public virtual IConnectionWapper GetOrCreate(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (!Connections.TryGetValue(connectionString, out var wapper))
            {
                wapper = _factory.CreateConnection(connectionString);
                Connections.Add(connectionString, wapper);
            }

            return wapper;
        }

        public virtual void Commit()
        {
            foreach (var connection in Connections.Values)
            {
                if (connection.HasTransaction && connection.Transaction.Commitable && !connection.Transaction.IsCommitted)
                {
                    connection.Transaction.Commit();
                }
            }
        }

        public virtual void Rollback()
        {
            foreach (var connection in Connections.Values)
            {
                if (connection.HasTransaction && connection.Transaction.Commitable && connection.Transaction.IsCommitted)
                {
                    connection.Transaction.Rollback();
                }
            }
        }

        public virtual void Dispose()
        {
            foreach (var connection in Connections.Values)
            {
                connection.Dispose();
            }
        }
    }
}
