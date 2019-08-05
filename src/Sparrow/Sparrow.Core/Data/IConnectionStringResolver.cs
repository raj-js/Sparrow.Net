namespace Sparrow.Core.Data
{
    public interface IConnectionStringResolver
    {
        string GetConnectionString(ConnectionStringResolveArgs args);
    }
}
