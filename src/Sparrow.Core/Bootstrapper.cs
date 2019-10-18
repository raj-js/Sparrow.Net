using Autofac;
using Microsoft.AspNetCore.Builder;
using Sparrow.Core.Services;

namespace Sparrow.Core
{
    public static class Bootstrapper
    {
        public static void AddSparrow(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(CURLService<,,,,>)).AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        public static void UseSparrow(this IApplicationBuilder app)
        {

        }
    }
}
