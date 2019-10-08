namespace Sparrow.Core
{
    public interface IEntity<TPKey>
    {
        TPKey Id { get; set; }
    }
}
