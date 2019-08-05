using Sparrow.Data;

namespace Sparrow.Core.Data
{
    public interface IConnectionFactory
    {
        IConnectionWapper CreateConnection(string connectionString);
    }
}
