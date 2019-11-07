using System.Collections.Generic;

namespace Sparrow.IdentityServer.Models.Consent
{
    /// <summary>
    /// 授权视图模型
    /// </summary>
    public class ConsentViewModel : ConsentFormModel
    {
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 客户端 Url
        /// </summary>
        public string ClientUrl { get; set; }

        /// <summary>
        /// 客户端 Logo
        /// </summary>
        public string ClientLogoUri { get; set; }

        /// <summary>
        /// 是否允许记住授权
        /// </summary>
        public bool AllowRememberConsent { get; set; }

        /// <summary>
        /// 标识范围
        /// </summary>
        public IEnumerable<ScopeViewModel> IdentityResourcesScopes { get; set; }

        /// <summary>
        /// 资源范围
        /// </summary>
        public IEnumerable<ScopeViewModel> ApiResourcesScopes { get; set; }
    }
}
