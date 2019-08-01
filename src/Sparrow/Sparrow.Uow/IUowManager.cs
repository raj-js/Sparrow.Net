namespace Sparrow.Uow
{
    public interface IUowManager
    {
        IUowHandle Begin(UowOptions options);
    }
}
