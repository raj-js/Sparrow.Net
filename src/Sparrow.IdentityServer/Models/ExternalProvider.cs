namespace Sparrow.IdentityServer.Models
{
    /// <summary>
    /// 第三方登录方式
    /// </summary>
    public class ExternalProvider
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 认证模式
        /// </summary>
        public string AuthenticationScheme { get; set; }
    }
}
