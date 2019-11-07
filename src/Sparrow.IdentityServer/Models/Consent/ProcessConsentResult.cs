using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparrow.IdentityServer.Models.Consent
{
    /// <summary>
    /// 授权处理结果
    /// </summary>
    public class ProcessConsentResult
    {
        /// <summary>
        /// 是否为重定向
        /// </summary>
        public bool IsRedirect => RedirectUri != null;

        /// <summary>
        /// 重定向地址
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 是否已经显示视图
        /// </summary>
        public bool ShowView => ViewModel != null;

        /// <summary>
        /// 授权视图模型
        /// </summary>
        public ConsentViewModel ViewModel { get; set; }

        /// <summary>
        /// 是否有验证错误
        /// </summary>
        public bool HasValidationError => ValidationError != null;

        /// <summary>
        /// 验证错误
        /// </summary>
        public string ValidationError { get; set; }
    }
}
