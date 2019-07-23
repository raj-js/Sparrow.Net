using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sparrow.Core.Security.Models;
using System.Reflection;

namespace Sparrow.Core.Security
{
    public static class SecurityExtensions
    {
        /// <summary>
        /// 使用 Identity 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="migrateAsm">需要迁移的程序集， 默认为 NULL 不作迁移</param>
        public static void AddIdentity(this IServiceCollection services, string connectionString, Assembly migrateAsm = null)
        {
            services.AddDbContext<IdentityContext>(
                options => 
                {
                    options.UseSqlServer(connectionString, sql => 
                    {
                        if (migrateAsm != null)
                            sql.MigrationsAssembly(migrateAsm.GetName().Name);
                    });
                });

            services.AddDefaultIdentity<User>()
                .AddEntityFrameworkStores<IdentityContext>();
        }

        public static void UseIdentity(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
        }
    }
}
