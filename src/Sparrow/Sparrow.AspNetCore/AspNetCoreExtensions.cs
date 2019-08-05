using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sparrow.Core;
using System;

namespace Sparrow.AspNetCore
{
    public static class AspNetCoreExtensions
    {
        public static IServiceProvider AddSparrow(this IServiceCollection services, Action<BootstrapperOptions> actionOptions = null)
        {
            var bootstrapper = new Bootstrapper(actionOptions);
            services.AddSingleton(bootstrapper);
            return WindsorRegistrationHelper.CreateServiceProvider(bootstrapper.IocManager.IocContainer, services);
        }

        public static void UseSparrow(this IApplicationBuilder app)
        {
            var bootstrapper = app.ApplicationServices.GetRequiredService<Bootstrapper>();

            bootstrapper.Initialize();
        }
    }
}
