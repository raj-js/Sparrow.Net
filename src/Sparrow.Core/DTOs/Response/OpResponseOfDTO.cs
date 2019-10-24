namespace Blog.Core.Sparrow.DTOs.Response
{
    public class OpResponse<TDTO> : OpResponse
    {
        public TDTO Data { get; set; }
    }
}
