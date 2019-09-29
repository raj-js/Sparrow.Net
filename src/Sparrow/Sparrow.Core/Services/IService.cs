namespace Sparrow.Core.Services
{
    public interface IService<IEntity, TPKey> where IEntity : class, IEntity<TPKey>
    {

    }
}
