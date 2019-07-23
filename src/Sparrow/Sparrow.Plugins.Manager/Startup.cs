using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sparrow.Core.Plugins;
using Sparrow.Core.Security;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

namespace Sparrow.Plugins.Manager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("IdentityContextConnection");
            services.AddIdentity(connectionString, migrateAsm: GetType().Assembly);

            var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            var plugins = Directory.EnumerateFiles(root, "Sparrow.Plugins.*.dll")
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                .ToArray();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .UsePlugins(plugins);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            SecurityExtensions.UseIdentity(app);
            app.UseMvcWithDefaultRoute();
        }
    }
}
