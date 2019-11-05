using System.ComponentModel.DataAnnotations;

namespace Sparrow.IdentityServer.Models
{
    /// <summary>
    /// 双因素登录
    /// </summary>
    public class LoginWith2faViewModel
    {
        /// <summary>
        /// 双因素代码
        /// </summary>
        [Required]
        [StringLength(7, ErrorMessage = "")]
        [DataType(DataType.Text)]
        [Display(Name = "双因素代码")]
        public string TwoFactorCode { get; set; }

        /// <summary>
        /// 记住机器
        /// </summary>
        [Display(Name = "记住机器")]
        public bool RememberMachine { get; set; }

        /// <summary>
        /// 记住账号
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// 返回 Url
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
