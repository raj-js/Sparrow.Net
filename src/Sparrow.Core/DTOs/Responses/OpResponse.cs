using Sparrow.Core.DTOs;
using System.Collections.Generic;

namespace Sparrow.Core.DTOs.Responses
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

        public static OpResponse Failure(Error error) 
        {
            var resp = Assert(false);
            resp.Errors.Add(error);
            return resp;
        }

        public static OpResponse Failure(string code, string msg)
        {
            var resp = Assert(false);
            resp.AddError(code, msg);
            return resp;
        }

        public static OpResponse<TDTO> Failure<TDTO>() => new OpResponse<TDTO>() { Status = -1 };

        public static OpResponse Success() => Assert(true);

        public static OpResponse<TDTO> Success<TDTO>(TDTO dto) => new OpResponse<TDTO>() { Data = dto };
    }
}
