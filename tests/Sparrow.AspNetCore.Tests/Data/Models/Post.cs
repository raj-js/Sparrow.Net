using System;
using Sparrow.Core.Domain.Entities;

namespace Sparrow.AspNetCore.Tests.Data.Models
{
    public class Post : Entity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PostTime { get; set; }

        public string BlogId { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
