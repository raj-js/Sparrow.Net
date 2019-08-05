using Sparrow.Core.Dependency;
using System;

namespace Sparrow.Core
{
    public class Bootstrapper : IDisposable
    {
        public IIocManager IocManager { get; }

        public BootstrapperOptions Options  { get; }

        public Bootstrapper(Action<BootstrapperOptions> actionOptions = null)
        {
            Options = new BootstrapperOptions();
            actionOptions?.Invoke(Options);

            IocManager = Options.IocManager;
        }

        public void Dispose()
        {
            IocManager.Dispose();
        }
    }
}
