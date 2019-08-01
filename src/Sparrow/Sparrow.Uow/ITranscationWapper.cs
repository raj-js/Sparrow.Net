using System.Data;

namespace Sparrow.Uow
{
    public interface ITranscationWapper
    {
        string ConnectionString { get; }

        IDbConnection DbConnection { get; }

        IDbTransaction Begin();
    }
}
