namespace Sparrow.Core.System
{
    public class ApplicationProp : IEntity<int>
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
