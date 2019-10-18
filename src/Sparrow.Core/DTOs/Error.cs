namespace Sparrow.Core.DTOs
{
    /// <summary>
    /// 错误
    /// </summary>
    public class Error
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Msg { get; set; }

        public static Error Create(string code, string msg) => new Error()
        {
            Code = code,
            Msg = msg
        };
    }
}
