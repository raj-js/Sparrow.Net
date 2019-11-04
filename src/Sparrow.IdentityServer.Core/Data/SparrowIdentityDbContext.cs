using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sparrow.IdentityServer.Core.Models;

namespace Sparrow.IdentityServer.Core.Data
{
    /// <summary>
    /// 应用标识数据上下文
    /// </summary>
    public class SparrowIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public SparrowIdentityDbContext()
        {

        }

        public SparrowIdentityDbContext(DbContextOptions<SparrowIdentityDbContext> options )
            : base(options)
        {

        }
    }
}
