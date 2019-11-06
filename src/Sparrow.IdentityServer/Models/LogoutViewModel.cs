namespace Sparrow.IdentityServer.Models
{
    /// <summary>
    /// 用户登出视图模型
    /// </summary>
    public class LogoutViewModel : LogoutFormModel
    {
        /// <summary>
        /// 是否显示提示框
        /// </summary>
        public bool ShowLogoutPrompt { get; set; }
    }
}
