using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sparrow.IdentityServer.Core.Data;
using Sparrow.IdentityServer.Core.Models;
using System;

namespace Sparrow.IdentityServer.Core.Configurations
{
    public static class IdentityExtensions
    {
        /// <summary>
        /// 添加 Sparrow Identity
        /// </summary>
        /// <param name="services"></param>
        public static IIdentityServerBuilder AddSparrowIdentity(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> actionIdentityOpt,
            Action<DbContextOptionsBuilder> actionConfigOpt,
            Action<DbContextOptionsBuilder> actionPersistedOpt
            ) 
        {
            services.AddDbContext<SparrowIdentityDbContext>(actionIdentityOpt);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SparrowIdentityDbContext>()
                .AddDefaultTokenProviders();

            return services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore<SparrowConfigurationDbContext>(opt => opt.ConfigureDbContext = actionConfigOpt)
                .AddOperationalStore<SparrowPersistedGrantDbContext>(opt => opt.ConfigureDbContext = actionPersistedOpt)
                .AddDeveloperSigningCredential();
        }

        public static void UseSparrowIdentity(this IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }
    }
}
