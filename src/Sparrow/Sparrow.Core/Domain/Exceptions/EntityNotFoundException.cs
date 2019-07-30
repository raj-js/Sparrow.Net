using System;

namespace Sparrow.Core.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public Type Type { get; }

        public object Id { get; }

        public EntityNotFoundException(Type type, object id)
        {
            Type = type;
            Id = id;
        }
    }
}
