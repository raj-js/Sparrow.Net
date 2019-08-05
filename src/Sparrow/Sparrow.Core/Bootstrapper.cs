using Castle.MicroKernel.Registration;
using Sparrow.Core.Dependency;
using System;

namespace Sparrow.Core
{
    public class Bootstrapper : IDisposable
    {
        public IIocManager IocManager { get; }

        public BootstrapperOptions Options  { get; }

        public Bootstrapper(Action<BootstrapperOptions> actionOptions = null)
        {
            Options = new BootstrapperOptions();
            actionOptions?.Invoke(Options);

            IocManager = Options.IocManager;
        }

        public virtual void Initialize()
        {
            if (!IocManager.IsRegistered<Bootstrapper>())
                IocManager.IocContainer.Register(Component.For<Bootstrapper>().Instance(this));

            IocManager.AddConventionalRegistrar(new BasicConventionalRegistrar());
            IocManager.Register<IScopedIocResolver, ScopedIocResolver>(DependencyLifeStyle.Transient);
        }

        public virtual void Dispose()
        {
            IocManager.Dispose();
        }
    }
}
