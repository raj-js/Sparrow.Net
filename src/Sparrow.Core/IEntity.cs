using System;

namespace Sparrow.Core
{
    public interface IEntity<TPKey> where TPKey : IEquatable<TPKey>
    {
        TPKey Id { get; set; }
    }
}
