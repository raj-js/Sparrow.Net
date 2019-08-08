using Sparrow.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Sparrow.AspNetCore.Tests.Data.Models
{
    public class Blog : Entity<string>
    {
        public string Author { get; set; }

        public string Signature { get; set; }

        public DateTime CreationTime { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
