using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Sparrow.AspNetCore.Tests.Data;
using Sparrow.EntityFrameworkCore;

namespace Sparrow.AspNetCore.Tests
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Default");

            var assembly = Assembly.GetExecutingAssembly();

            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseSqlServer(connectionString, 
                    setup => setup.MigrationsAssembly(assembly.FullName));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services.AddSparrow(options =>
            {
                options.IocManager.AddEfCoreDependencies(assembly);
            });
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

            app.UseHttpsRedirection();
            app.UseSparrow();
            app.UseMvc();
        }
    }
}
