using IdentityServer4.Stores;
using System.Threading.Tasks;

namespace Sparrow.IdentityServer
{
    public static class Extensions
    {
        /// <summary>
        /// 判断客户端是否使用 PKCE
        /// </summary>
        /// <param name="clientStore"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static async Task<bool> IsPkceClientAsync(this IClientStore clientStore, string clientId)
        {
            if (string.IsNullOrEmpty(clientId) == false) 
            {
                // 查找启用的客户端
                var client = await clientStore.FindEnabledClientByIdAsync(clientId);
                return client?.RequirePkce == true;
            }

            return false;
        }
    }
}
