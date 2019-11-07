using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparrow.IdentityServer.Models.Consent
{
    /// <summary>
    /// 授权选项
    /// </summary>
    public class ConsentOptions
    {
        /// <summary>
        /// 是否启用离线访问
        /// </summary>
        public static bool EnableOfflineAccess { get; set; } = true;

        /// <summary>
        /// 离线访问权限显示名称
        /// </summary>
        public static string OfflineAccessDisplayName { get; set; } = "离线访问";

        /// <summary>
        /// 离线访问权限描述
        /// </summary>
        public static string OfflineAccessDescription { get; set; } = "使客户端拥有对您的应用程序与资源离线访问的权限";


        /// <summary>
        /// 至少选择一项权限
        /// </summary>
        public const string MustChooseOneErrorMessage = "至少选择一项权限";

        /// <summary>
        /// 无效的选项
        /// </summary>
        public const string InvalidSelectionErrorMessage = "无效的选项";
    }
}
