using Sparrow.Core.DTOs;
using System.Collections.Generic;

namespace Blog.Core.Sparrow.DTOs.Response
{
    public class OpResponse
    {
        public int Status { get; set; }

        public string Msg { get; set; }

        public List<Error> Errors { get; set; } = new List<Error>();

        public bool IsSuccess => Status == default;

        public static OpResponse Assert(bool isSuccess)
        {
            return isSuccess ?
                new OpResponse() :
                new OpResponse() { Status = -1 };
        }

        public static OpResponse Failure() => Assert(false);

        public static OpResponse<TDTO> Failure<TDTO>() => new OpResponse<TDTO>() { Status = -1 };

        public static OpResponse Success() => Assert(true);

        public static OpResponse<TDTO> Success<TDTO>(TDTO dto) => new OpResponse<TDTO>() { Data = dto };
    }
}
