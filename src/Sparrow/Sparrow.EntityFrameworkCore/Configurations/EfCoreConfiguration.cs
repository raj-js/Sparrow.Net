using Castle.MicroKernel.Registration;
using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Dependency;
using System;

namespace Sparrow.EntityFrameworkCore.Configurations
{
    public class EfCoreConfiguration : IEfCoreConfiguration
    {
        private readonly IIocManager _iocManager;

        public EfCoreConfiguration(IIocManager iocManager)
        {
            _iocManager = iocManager;
        }

        public void AddDbContext<TDbContext>(Action<EfDbContextConfiguration<TDbContext>> action) where TDbContext : DbContext
        {
            _iocManager.IocContainer.Register(
                Component.For<IEfDbContextConfigurator<TDbContext>>().Instance(
                    new EfDbContextConfigurator<TDbContext>(action)
                ).IsDefault()
            );
        }
    }
}
