using Sparrow.Core.DTOs;

namespace Sparrow.Core.DTOs.Responses
{
    public static class OpResponseExtensions
    {
        public static void AddError(this OpResponse response, string code, string msg)
        {
            response.Errors.Add(Error.Create(code, msg));
        }
    }
}
