using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Domain.Uow;
using Sparrow.EntityFrameworkCore.Uow;

namespace Sparrow.EntityFrameworkCore
{
    public class DbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        private readonly ICurrentUowProvider _currentUowProvider;

        public DbContextProvider(ICurrentUowProvider currentUowProvider)
        {
            _currentUowProvider = currentUowProvider;
        }

        public TDbContext GetDbContext()
        {
            return ((EfCoreUow)_currentUowProvider.Current).GetDbContext<TDbContext>();
        }
    }
}
