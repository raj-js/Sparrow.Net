using Castle.MicroKernel.Registration;
using Sparrow.Core.Dependency;
using System;
using System.Reflection;
using Sparrow.Core.Domain.Uow;

namespace Sparrow.Core
{
    public class Bootstrapper : IDisposable
    {
        public IIocManager IocManager { get; }

        public BootstrapperOptions Options  { get; }

        public Bootstrapper(
            Action<IIocManager, BootstrapperOptions> preInitialize = null, 
            Action<BootstrapperOptions> initialize = null
            )
        {
            Options = new BootstrapperOptions();

            IocManager = Options.IocManager;

            IocManager.AddConventionalRegistrar(new BasicConventionalRegistrar());

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.Register<IScopedIocResolver, ScopedIocResolver>(DependencyLifeStyle.Transient);

            IocManager.IocContainer.Register(
                Component.For<UowOptions>()
                    .Instance(Options.UowOptions)
                    .LifeStyle.Singleton
                );

            IocManager.IocContainer.Register(Component.For<Bootstrapper>().Instance(this));

            preInitialize?.Invoke(IocManager, Options);

            UowRegistrar.Initialize(IocManager);

            initialize?.Invoke(Options);
        }

        public virtual void Initialize()
        {
            
        }

        public virtual void Dispose()
        {
            IocManager.Dispose();
        }
    }
}
