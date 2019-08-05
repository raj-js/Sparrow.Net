using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Sparrow.Core;
using System;

namespace Sparrow.AspNetCore
{
    public static class AspNetCoreExtensions
    {
        public static IServiceProvider AddSparrowCore(this IServiceCollection services, Action<BootstrapperOptions> actionOptions = null)
        {
            var bootstrapper = new Bootstrapper(actionOptions);
            services.AddSingleton(bootstrapper);
            return WindsorRegistrationHelper.CreateServiceProvider(bootstrapper.IocManager.IocContainer, services);
        }
    }
}
