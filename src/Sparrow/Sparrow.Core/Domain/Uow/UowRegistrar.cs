using Castle.Core;
using Castle.MicroKernel;
using Sparrow.Core.Dependency;
using System.Linq;
using System.Reflection;

namespace Sparrow.Core.Domain.Uow
{
    internal static class UowRegistrar
    {
        public static void Initialize(IIocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += (key, handler) =>
            {
                var implementationType = handler.ComponentModel.Implementation.GetTypeInfo();

                HandleTypesWithUowAttribute(implementationType, handler);
                HandleConventionalUowTypes(implementationType, handler);
            };
        }

        private static void HandleTypesWithUowAttribute(TypeInfo implementationType, IHandler handler)
        {
            if (IsUnitOfWorkType(implementationType) || AnyMethodHasUnitOfWork(implementationType))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(UowInterceptor)));
            }
        }

        private static void HandleConventionalUowTypes(TypeInfo implementationType, IHandler handler)
        {
            if (UowOptions.Default.IsConventionalUowClass(implementationType.AsType()))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(UowInterceptor)));
            }
        }

        private static bool IsUnitOfWorkType(TypeInfo implementationType)
        {
            return UowHelper.HasUnitOfWorkAttribute(implementationType);
        }

        private static bool AnyMethodHasUnitOfWork(TypeInfo implementationType)
        {
            return implementationType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(UowHelper.HasUnitOfWorkAttribute);
        }
    }
}
