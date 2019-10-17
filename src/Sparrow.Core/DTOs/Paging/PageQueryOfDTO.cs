namespace Sparrow.Core.DTOs.Paging
{
    public class PageQuery<TDTO> : PageQuery
    {
        public TDTO Query { get; set; }
    }
}
