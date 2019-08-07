using Microsoft.EntityFrameworkCore;
using System;

namespace Sparrow.EntityFrameworkCore.Configurations
{
    public interface IEfCoreConfiguration
    {
        void AddDbContext<TDbContext>(Action<EfDbContextConfiguration<TDbContext>> action)
            where TDbContext : DbContext;
    }
}
