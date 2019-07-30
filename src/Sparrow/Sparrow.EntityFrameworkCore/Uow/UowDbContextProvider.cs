using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Domain.Uow;

namespace Sparrow.EntityFrameworkCore.Uow
{
    public class UowDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        private readonly ICurrentUowProvider _currentUowProvider;

        public UowDbContextProvider(ICurrentUowProvider currentUowProvider)
        {
            _currentUowProvider = currentUowProvider;
        }

        public TDbContext GetDbContext(string name = null)
        {
            return _currentUowProvider.Current.GetDbContext<TDbContext>(name);
        }
    }
}
