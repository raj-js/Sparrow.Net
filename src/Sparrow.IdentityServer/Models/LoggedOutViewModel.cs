using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparrow.IdentityServer.Models
{
    /// <summary>
    /// 用户已登出视图模型
    /// </summary>
    public class LoggedOutViewModel
    {
        /// <summary>
        /// 登出后跳转地址
        /// </summary>
        public string PostLogoutRedirectUri { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 登出后 iframe 的 url
        /// </summary>
        public string SignOutIFrameUrl { get; set; }

        /// <summary>
        /// 登出后是否自动跳转
        /// </summary>
        public bool AutomaticRedirectAfterSignOut { get; set; }

        /// <summary>
        /// 登出标识
        /// </summary>
        public string LogoutId { get; set; }

        /// <summary>
        /// 是否触发外部登出
        /// </summary>
        public bool TriggerExternalSignout => ExternalAuthenticationSchema != null;

        /// <summary>
        /// 外部认证模式
        /// </summary>
        public string ExternalAuthenticationSchema { get; set; }
    }
}
