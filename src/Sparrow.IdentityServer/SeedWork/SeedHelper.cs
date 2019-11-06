using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sparrow.IdentityServer.Core.Data;
using Sparrow.IdentityServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

                await AddUsersAsync(configuration, userManager, logger);
            }
        }

        #region privates

        /// <summary>
        /// 加载种子用户
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private static List<SeedUser> LoadSeedUsers(IConfiguration configuration)
        {
            var usersSection = configuration.GetSection("SeedData:Users");

            var seedUsers = new List<SeedUser>();
            usersSection.Bind(seedUsers);

            return seedUsers;
        }

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
            var seedUsers = LoadSeedUsers(configuration);

            foreach (var seedUser in seedUsers)
            {
                var user = await userManager.FindByIdAsync(seedUser.Id);

                if (user != null)
                    continue;

                var identityResult = await userManager.CreateAsync(new ApplicationUser
                {
                    Id = seedUser.Id,
                    UserName = seedUser.UserName,
                    Email = seedUser.Email,
                    EmailConfirmed = true,
                    PhoneNumber = seedUser.Phone,
                    PhoneNumberConfirmed = true
                }, seedUser.Password);

                if (identityResult.Succeeded == false)
                    logger.LogError($"新增 seed user 失败: {identityResult.ErrorToString()}");
            }
        }

        private static void AddIdentityResource(SparrowConfigurationDbContext context) 
        {
            var resources = Config.GetIdentityResources();

            foreach (var resource in resources)
            {
                context.IdentityResources.Add();
            }
        }

        private static void AddApiResources()
        {
            
        }

        private static void AddClients(SparrowConfigurationDbContext context)
        {
            
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
