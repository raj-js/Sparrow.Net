namespace Sparrow.Core.DTOs.Paging
{
    public class PageQuery
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Order { get; set; }

        public bool IsAsc { get; set; }
    }
}
