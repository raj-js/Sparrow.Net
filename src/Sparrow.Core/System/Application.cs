namespace Sparrow.Core.System
{
    public class Application : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }
    }
}
