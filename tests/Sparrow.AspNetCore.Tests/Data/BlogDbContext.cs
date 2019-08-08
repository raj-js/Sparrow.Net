using Microsoft.EntityFrameworkCore;
using Sparrow.AspNetCore.Tests.Data.Models;

namespace Sparrow.AspNetCore.Tests.Data
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }
    }
}
