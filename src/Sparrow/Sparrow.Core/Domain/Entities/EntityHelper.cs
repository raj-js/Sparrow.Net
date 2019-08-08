using Sparrow.Core.Reflections;
using System;
using System.Reflection;

namespace Sparrow.Core.Domain.Entities
{
    public static class EntityHelper
    {
        public static bool IsEntity(Type type)
        {
            return ReflectionHelper.IsAssignableToGenericType(type, typeof(IEntity<>));
        }

        public static Type GetPrimaryKeyType<TEntity>()
        {
            return GetPrimaryKeyType(typeof(TEntity));
        }

        /// <summary>
        /// Gets primary key type of given entity type
        /// </summary>
        public static Type GetPrimaryKeyType(Type entityType)
        {
            foreach (var interfaceType in entityType.GetTypeInfo().GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
                {
                    return interfaceType.GenericTypeArguments[0];
                }
            }

            throw new Exception("Can not find primary key type of given entity type: " + entityType + ". Be sure that this entity type implements IEntity<TPrimaryKey> interface");
        }

        public static object GetEntityId(object entity)
        {
            if (!ReflectionHelper.IsAssignableToGenericType(entity.GetType(), typeof(IEntity<>)))
            {
                throw new Exception(entity.GetType() + " is not an Entity !");
            }

            return ReflectionHelper.GetValueByPath(entity, entity.GetType(), "Id");
        }
    }
}
