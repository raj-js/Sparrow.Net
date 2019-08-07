using Microsoft.EntityFrameworkCore;

namespace Sparrow.EntityFrameworkCore.Configurations
{
    public interface IEfDbContextConfigurator<TDbContext> where TDbContext : DbContext
    {
        void Configure(EfDbContextConfiguration<TDbContext> configuration);
    }
}
