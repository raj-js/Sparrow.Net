using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Dependency;
using Sparrow.Core.Domain.Entities;
using Sparrow.Core.Reflections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sparrow.EntityFrameworkCore
{
    internal class EfCoreDbContextEntityFinder : IDbContextEntityFinder, ITransientDependency
    {
        public IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType)
        {
            return
                from property in dbContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0], typeof(IEntity<>))
                select new EntityTypeInfo(property.PropertyType.GenericTypeArguments[0], property.DeclaringType);
        }
    }
}
