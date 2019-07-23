using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sparrow.Core.Plugins;
using Sparrow.Core.Security;
using Sparrow.Core.Security.Models;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

namespace Sparrow.Plugins.Identity
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
            services.AddIdentity(Configuration.GetConnectionString("IdentityContextConnection"));

            var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            var plugins = Directory.EnumerateFiles(root, "Sparrow.Plugins.Manager.dll")
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                .ToArray();

            services.AddMvc()
                .UsePlugins(plugins)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            SecurityExtensions.UseIdentity(app);
            app.UseMvcWithDefaultRoute();
        }
    }
}
