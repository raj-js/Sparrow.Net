namespace Sparrow.Core.Domain.Uow
{
    public interface IConnectionStringResolver
    {
        string GetNameOrConnectionString(ConnectionStringResolveArgs args);
    }
}
