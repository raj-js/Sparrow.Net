using System;
using System.Linq;
using System.Reflection;

namespace Sparrow.Core.Domain.Uow
{
    internal static class UowOptionsExtensions
    {
        public static UowAttribute GetUnitOfWorkAttributeOrNull(this UowOptions uowOptions, MethodInfo methodInfo)
        {
            var attrs = methodInfo
                .GetCustomAttributes(true)
                .OfType<UowAttribute>()
                .ToArray();

            if (attrs.Length > 0) return attrs[0];

            attrs = methodInfo.DeclaringType
                .GetTypeInfo()
                .GetCustomAttributes(true)
                .OfType<UowAttribute>()
                .ToArray();

            if (attrs.Length > 0) return attrs[0];

            return uowOptions.IsConventionalUowClass(methodInfo.DeclaringType) ? new UowAttribute() : null;
        }

        public static bool IsConventionalUowClass(this UowOptions uowOptions, Type type)
        {
            return uowOptions.ConventionalUowSelectors.Any(selector => selector(type));
        }
    }
}
