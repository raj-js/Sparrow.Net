using System.Reflection;

namespace Sparrow.Core.Domain.Uow
{
    internal static class UowHelper
    {
        public static bool HasUnitOfWorkAttribute(MemberInfo memberInfo)
        {
            return memberInfo.IsDefined(typeof(UowAttribute), true);
        }
    }
}
