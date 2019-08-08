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

        public Bootstrapper(Action<BootstrapperOptions> actionOptions = null)
        {
            Options = new BootstrapperOptions();
            IocManager = Options.IocManager;

            IocManager.AddConventionalRegistrar(new BasicConventionalRegistrar());

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.Register<IScopedIocResolver, ScopedIocResolver>(DependencyLifeStyle.Transient);

            UowRegistrar.Initialize(IocManager);

            actionOptions?.Invoke(Options);
        }

        public virtual void Initialize()
        {
            if (!IocManager.IsRegistered<Bootstrapper>())
                IocManager.IocContainer.Register(Component.For<Bootstrapper>().Instance(this));
        }

        public virtual void Dispose()
        {
            IocManager.Dispose();
        }
    }
}
