using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Sparrow.IdentityServer.SeedWork
{
    public class Config
    {
        /// <summary>
        /// Identity 资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile() ,
            };
        }

        /// <summary>
        /// Api 资源
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources(IConfiguration configuration)
        {
            var apiResources = new List<ApiResource>();

            BindFromConfig(configuration, "SeedData:ApiResources", ref apiResources);

            return apiResources;
        }

        /// <summary>
        /// 客户端
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var clients = new List<Client>();

            BindFromConfig(configuration, "SeedData:Clients", ref clients);

            return clients;
        }

        /// <summary>
        /// 用户
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IEnumerable<SeedUser> GetUsers(IConfiguration configuration)
        {
            var users = new List<SeedUser>();

            BindFromConfig(configuration, "SeedData:Users", ref users);

            return users;
        }

        #region privates

        private static void BindFromConfig<T>(IConfiguration configuration, string key, ref T obj)
        {
            var section = configuration.GetSection(key);

            if (section == null) return;

            section.Bind(obj);
        }

        #endregion
    }
}
