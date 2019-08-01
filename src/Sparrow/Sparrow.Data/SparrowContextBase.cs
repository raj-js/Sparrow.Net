﻿using System;
using System.Collections.Generic;

namespace Sparrow.Data
{
    public abstract class SparrowContextBase : ISparrowContext
    {
        protected Dictionary<string, IConnectionWapper> Connections;

        public SparrowContextBase()
        {
            Connections = new Dictionary<string, IConnectionWapper>();
        }

        protected abstract IConnectionWapper CreateConnection(string connectionString);

        public virtual IConnectionWapper GetOrCreate(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (!Connections.TryGetValue(connectionString, out var connection))
            {
                connection = CreateConnection(connectionString);
            }

            return connection;
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
