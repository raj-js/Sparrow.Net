namespace Sparrow.Core.DTOs.Responses
{
    public class OpResponse<TDTO> : OpResponse
    {
        public TDTO Data { get; set; }
    }
}
