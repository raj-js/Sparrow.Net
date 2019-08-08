using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sparrow.Core;
using System;

namespace Sparrow.AspNetCore
{
    public static class AspNetCoreExtensions
    {
        public static IServiceProvider AddSparrow(this IServiceCollection services, 
            Action<BootstrapperOptions> initialize = null
            )
        {
            var bootstrapper = new Bootstrapper(preInitialize: (iocManager, options) =>
                {
                    iocManager.IocContainer.Register(
                        Component.For<ControllerBase>().LifestyleTransient()
                    );
                    options.UowOptions.ConventionalUowSelectors.Add(type=>typeof(ControllerBase).IsAssignableFrom(type));
                }, 
                initialize);
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
