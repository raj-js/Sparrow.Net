using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sparrow.EntityFrameworkCore
{
    public class DbContextTypeMatcher : IDbContextTypeMatcher
    {
        private readonly Dictionary<Type, List<Type>> _dbContextTypes;

        protected DbContextTypeMatcher()
        {
            _dbContextTypes = new Dictionary<Type, List<Type>>();
        }

        public void Populate(Type[] dbContextTypes)
        {
            foreach (var dbContextType in dbContextTypes)
            {
                var types = new List<Type>();

                AddWithBaseTypes(dbContextType, types);

                foreach (var type in types)
                {
                    Add(type, dbContextType);
                }
            }
        }

        public virtual Type GetConcreteType(Type sourceDbContextType)
        {
            if (!sourceDbContextType.GetTypeInfo().IsAbstract)
            {
                return sourceDbContextType;
            }

            if (_dbContextTypes.TryGetValue(sourceDbContextType, out var allTargetTypes) && allTargetTypes.Count == 1)
            {
                return allTargetTypes[0];
            }

            throw new Exception("Could not find a concrete implementation of given DbContext type: " + sourceDbContextType.AssemblyQualifiedName);
        }

        private static void AddWithBaseTypes(Type dbContextType, List<Type> types)
        {
            types.Add(dbContextType);
            if (dbContextType != typeof(DbContext))
            {
                AddWithBaseTypes(dbContextType.GetTypeInfo().BaseType, types);
            }
        }

        private void Add(Type sourceDbContextType, Type targetDbContextType)
        {
            if (!_dbContextTypes.ContainsKey(sourceDbContextType))
            {
                _dbContextTypes[sourceDbContextType] = new List<Type>();
            }

            _dbContextTypes[sourceDbContextType].Add(targetDbContextType);
        }
    }
}
