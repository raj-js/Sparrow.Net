namespace Sparrow.Core.DTOs.Paging
{
    public class PageQuery
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string Order { get; set; } = "Id";

        public bool IsAsc { get; set; } = true;
    }
}
