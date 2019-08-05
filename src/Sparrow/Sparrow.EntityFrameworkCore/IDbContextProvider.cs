using Microsoft.EntityFrameworkCore;

namespace Sparrow.EntityFrameworkCore
{
    public interface IDbContextProvider<TDbContext> where TDbContext : DbContext
    {
        TDbContext GetDbContext();
    }
}
