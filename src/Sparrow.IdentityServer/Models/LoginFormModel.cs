namespace Sparrow.IdentityServer.Models
{
    /// <summary>
    /// 登录表单模型
    /// </summary>
    public class LoginFormModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 记住账号
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// 返回 url
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
