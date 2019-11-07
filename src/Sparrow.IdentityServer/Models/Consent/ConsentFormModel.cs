using System.Collections.Generic;

namespace Sparrow.IdentityServer.Models.Consent
{
    /// <summary>
    /// 授权表单模型
    /// </summary>
    public class ConsentFormModel
    {
        /// <summary>
        /// 同意或者拒绝
        /// </summary>
        public string Button { get; set; }

        /// <summary>
        /// 同意授予给客户端的访问范围
        /// </summary>
        public IEnumerable<string> ScopesConsented { get; set; }

        /// <summary>
        /// 记住授权
        /// </summary>
        public bool RememberConsent { get; set; }

        /// <summary>
        /// 返回客户端的Url
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
