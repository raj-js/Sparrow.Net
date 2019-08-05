using Microsoft.EntityFrameworkCore;
using System;

namespace Sparrow.EntityFrameworkCore
{
    public class EfDbContextConfigurator<TDbContext> : IEfDbContextConfigurator<TDbContext>
        where TDbContext : DbContext
    {
        private Action<EfDbContextConfiguration<TDbContext>> _action;

        public EfDbContextConfigurator(Action<EfDbContextConfiguration<TDbContext>> action)
        {
            _action = action;
        }

        public void Configure(EfDbContextConfiguration<TDbContext> configuration)
        {
            _action(configuration);
        }
    }
}
