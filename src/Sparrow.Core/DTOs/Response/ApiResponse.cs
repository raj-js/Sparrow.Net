using System.Collections.Generic;

namespace Blog.Core.Sparrow.DTOs.Response
{
    public class ApiResponse
    {
        public int Status { get; set; }

        public string Msg { get; set; }

        public List<(string Code, string Msg)> Errors { get; set; }

        public ApiResponse()
        {
            Errors = new List<(string Code, string Msg)>();
        }

        public static ApiResponse Assert(bool isSuccess)
        {
            return isSuccess ? 
                new ApiResponse() : 
                new ApiResponse() { Status = -1 };
        }

        public static ApiResponse<TDTO> Success<TDTO>(TDTO dto)
        {
            return new ApiResponse<TDTO>()
            {
                Data = dto
            };
        }
    }
}
