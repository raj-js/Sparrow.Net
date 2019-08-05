using Sparrow.Core.Dependency;

namespace Sparrow.Core
{
    public class BootstrapperOptions
    {
        public IIocManager IocManager { get; set; }

        public BootstrapperOptions()
        {
            IocManager = Dependency.IocManager.Instance;
        }
    }
}
