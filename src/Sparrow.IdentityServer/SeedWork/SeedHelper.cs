using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sparrow.IdentityServer.Core.Data;
using Sparrow.IdentityServer.Core.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace Sparrow.IdentityServer.SeedWork
{
    public static class SeedHelper
    {
        /// <summary>
        /// 添加种子值
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static async Task AddSeedDataAsync(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;

                var configuration = provider.GetRequiredService<IConfiguration>();
                var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
                var logger = provider.GetRequiredService<ILogger<Startup>>();
                var context = provider.GetRequiredService<SparrowConfigurationDbContext>();
                                       
                await AddUsersAsync(configuration, userManager, logger);

                await AddIdentityResourceAsync(context);

                await AddApiResourcesAsync(context, configuration);

                await AddClientsAsync(context, configuration);
            }
        }

        #region privates

        /// <summary>
        /// 添加种子用户
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        private static async Task AddUsersAsync(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            ILogger<Startup> logger
            )
        {
            var users = Config.GetUsers(configuration);

            foreach (var seed in users)
            {
                var user = await userManager.FindByIdAsync(seed.Id);

                if (user != null)
                    continue;

                var identityResult = await userManager.CreateAsync(new ApplicationUser
                {
                    Id = seed.Id,
                    UserName = seed.UserName,
                    Email = seed.Email,
                    EmailConfirmed = true,
                    PhoneNumber = seed.Phone,
                    PhoneNumberConfirmed = true
                }, seed.Password);

                if (identityResult.Succeeded == false)
                    logger.LogError($"新增 seed user 失败: {identityResult.ErrorToString()}");
            }
        }

        /// <summary>
        /// 添加 Identity 资源
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task AddIdentityResourceAsync(SparrowConfigurationDbContext context) 
        {
            var resources = Config.GetIdentityResources();

            foreach (var resource in resources)
            {
                if (context.IdentityResources.Any(r => r.Name == resource.Name)) 
                    continue;

                context.IdentityResources.Add(resource.ToEntity());
            }

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// 添加 Api 资源
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static async Task AddApiResourcesAsync(SparrowConfigurationDbContext context, IConfiguration configuration)
        {
            var apiResources = Config.GetApiResources(configuration);

            foreach (var apiResource in apiResources)
            {
                if (context.ApiResources.Any(r => r.Name == apiResource.Name))
                    continue;

                context.ApiResources.Add(apiResource.ToEntity());
            }

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <param name="context"></param>
        private static async Task AddClientsAsync(SparrowConfigurationDbContext context, IConfiguration configuration)
        {
            var clients = Config.GetClients(configuration);

            foreach (var client in clients)
            {
                if (context.Clients.Any(r => r.ClientId == client.ClientId))
                    continue;

                context.Clients.Add(client.ToEntity());
            }

            await context.SaveChangesAsync();
        }

        private static string ErrorToString(this IdentityResult identityResult)
        {
            if (identityResult.Succeeded) return "";

            var @string = new StringBuilder();

            foreach (var error in identityResult.Errors)
            {
                @string.AppendLine($"{error.Code}: {error.Description}");
            }

            return @string.ToString();
        }

        #endregion
    }
}
