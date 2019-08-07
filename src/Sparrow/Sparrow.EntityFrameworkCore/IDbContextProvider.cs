using Microsoft.EntityFrameworkCore;

namespace Sparrow.EntityFrameworkCore
{
    public interface IDbContextProvider<out TDbContext> where TDbContext : DbContext
    {
        TDbContext GetDbContext();
    }
}
