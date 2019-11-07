namespace Sparrow.IdentityServer.Models.Consent
{
    /// <summary>
    /// 访问范围视图模型
    /// </summary>
    public class ScopeViewModel
    {
        /// <summary>
        /// 范围名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 强调
        /// </summary>
        public bool Emphasize { get; set; }

        /// <summary>
        /// 必选
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// 选中
        /// </summary>
        public bool Checked { get; set; }
    }
}
