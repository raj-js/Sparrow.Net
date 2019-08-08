using Sparrow.Core.Dependency;
using Sparrow.Core.Domain.Uow;

namespace Sparrow.Core
{
    public class BootstrapperOptions
    {
        public IIocManager IocManager { get; private set; }

        public UowOptions UowOptions { get; private set; }

        public BootstrapperOptions()
        {
            IocManager = Dependency.IocManager.Instance;
            UowOptions = UowOptions.Default;
        }
    }
}
