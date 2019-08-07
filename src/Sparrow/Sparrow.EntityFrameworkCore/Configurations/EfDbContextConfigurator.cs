using System;
using Microsoft.EntityFrameworkCore;

namespace Sparrow.EntityFrameworkCore.Configurations
{
    public class EfDbContextConfigurator<TDbContext> : IEfDbContextConfigurator<TDbContext>
        where TDbContext : DbContext
    {
        private readonly Action<EfDbContextConfiguration<TDbContext>> _action;

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
