using System;

namespace Sparrow.IdentityServer.Models
{
    /// <summary>
    /// 账号选项
    /// </summary>
    public class AccountOptions
    {
        /// <summary>
        /// 允许本地登录
        /// </summary>
        public static bool AllowLocalLogin = true;

        /// <summary>
        /// 允许记住登录
        /// </summary>
        public static bool AllowRememberLogin = true;
        
        /// <summary>
        /// 记住登录持续时长
        /// </summary>
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        /// <summary>
        /// 显示注销提示
        /// </summary>
        public static bool ShowLogoutPrompt = true;

        /// <summary>
        /// 在注销后自动跳转
        /// </summary>
        public static bool AutomaticRedirectAfterSignOut = false;

        // specify the Windows authentication scheme being used
        public static readonly string WindowsAuthenticationSchemeName = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;

        // if user uses windows auth, should we load the groups from windows
        public static bool IncludeWindowsGroups = false;

        /// <summary>
        /// 无效的凭证
        /// </summary>
        public static string InvalidCredentialsErrorMessage = "账号或者密码错误";
    }
}
