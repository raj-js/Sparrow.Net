using Autofac;
using Sparrow.Core;
using Sparrow.EntityFrameworkCore.Stores;

namespace Sparrow.EntityFrameworkCore
{
    public static class StartUp
    {
        public static void AddEfCore(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EfStore<,>))
                .As(
                    typeof(IStore<,>),
                    typeof(IEfStore<,>)
                    );
        }
    }
}
