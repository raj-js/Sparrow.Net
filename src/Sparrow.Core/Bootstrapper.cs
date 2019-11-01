using Autofac;
using Microsoft.AspNetCore.Builder;
using Sparrow.Core.Services;

namespace Sparrow.Core
{
    public static class Bootstrapper
    {
        public static void AddSparrow(this ContainerBuilder builder)
        {
            builder.RegisterType(typeof(AppServiceBase)).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(AppServiceBase<,>)).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(AppServiceBase<,,,,>)).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(AppServiceBase<,,,,,>)).AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        public static void UseSparrow(this IApplicationBuilder app)
        {

        }
    }
}
