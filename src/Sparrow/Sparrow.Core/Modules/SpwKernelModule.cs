using Sparrow.Core.Dependency;
using Sparrow.Core.Domain.Uow;

namespace Sparrow.Core.Modules
{
    public sealed class SpwKernelModule : SpwModule
    {
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new BasicConventionalRegistrar());

            IocManager.Register<IScopedIocResolver, ScopedIocResolver>(DependencyLifeStyle.Transient);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SpwKernelModule).Assembly,
                new ConventionalRegistrationConfig
                {
                    InstallInstallers = false
                });
        }

        public override void PostInitialize()
        {
            RegisterMissingComponents();
        }

        public override void Shutdown()
        {

        }

        private void RegisterMissingComponents()
        {
            IocManager.RegisterIfNot<IUow, NullUow>(DependencyLifeStyle.Transient);
        }
    }
}
