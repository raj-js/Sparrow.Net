namespace Sparrow.Infrastructure.Paging
{
    public interface IQuery
    {
        int Skip { get; set; }

        int Take { get; set; }

        string SortBy { get; set; }

        bool Desc { get; set; }
    }
}
