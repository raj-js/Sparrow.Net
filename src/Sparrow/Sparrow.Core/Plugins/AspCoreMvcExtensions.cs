using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Sparrow.Core.Plugins
{
    public static class AspCoreMvcExtensions
    {
        /// <summary>
        /// Mvc 插件式开发
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="plugins"></param>
        /// <returns></returns>
        public static IMvcBuilder UsePlugins(this IMvcBuilder builder, params Assembly[] plugins)
        {
            builder.ConfigureApplicationPartManager(apm =>
            {
                foreach (var plugin in plugins)
                {
                    var appParts = new DefaultApplicationPartFactory().GetApplicationParts(plugin);
                    foreach (var part in appParts)
                        apm.ApplicationParts.Add(part);

                    var compiledParts = new CompiledRazorAssemblyApplicationPartFactory().GetApplicationParts(plugin);
                    foreach (var part in compiledParts)
                        apm.ApplicationParts.Add(part);
                }
            });
            return builder;
        }
    }
}
