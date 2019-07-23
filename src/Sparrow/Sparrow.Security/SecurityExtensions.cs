using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sparrow.Security.Data;
using Sparrow.Security.Models;
using System;

namespace Sparrow.Security
{
    public static class SecurityExtensions
    {
        /// <summary>
        /// 使用 Identity 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="assemblyName">需要迁移的程序集</param>
        /// <param name="identityOptions"></param>
        public static void AddSparrowIdentity(this IServiceCollection services,
            string connectionString,
            string assemblyName,
            Action<IdentityOptions> identityOptions = null
            )
        {
            services.AddDbContext<IdentityDbContext>(
                options =>
                {
                    options.UseSqlServer(connectionString, sql =>
                    {
                        sql.MigrationsAssembly(assemblyName);
                    });
                });

            var identityBuilder = identityOptions == null ?
                services.AddIdentity<User, Role>() :
                services.AddIdentity<User, Role>(identityOptions);

            identityBuilder.AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddAspNetIdentity<User>()
                .AddConfigurationStore<ConfigurationDbContext>(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(assemblyName));
                })
                .AddOperationalStore<PersistedGrantDbContext>(options =>
                {
                    options.EnableTokenCleanup = true;
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(assemblyName));
                })
                .AddDeveloperSigningCredential();   // 开发环境

            services.AddAuthentication();
        }

        public static void UseSparrowIdentity(this IApplicationBuilder app)
        {
            // app.UseHsts(options => options.MaxAge(days: 365));
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseIdentityServer();
        }
    }
}
