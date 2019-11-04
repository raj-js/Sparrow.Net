using System.Collections.Generic;
using System.Linq;

namespace Sparrow.IdentityServer.Models
{
    /// <summary>
    /// 登录视图模型
    /// </summary>
    public class LoginViewModel : LoginFormModel
    {
        /// <summary>
        /// 允许记住登录
        /// </summary>
        public bool AllowRememberLogin { get; set; } = true;

        /// <summary>
        /// 允许本地登录
        /// </summary>
        public bool EnableLocalLogin { get; set; } = true;

        /// <summary>
        /// 第三方登录方式
        /// </summary>
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();

        /// <summary>
        /// 可见的第三方登录方式
        /// </summary>
        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

        // 是否仅为第三方登录        
        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        
        /// <summary>
        /// 第三方登录模式
        /// </summary>
        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
    }
}
