using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Domain.Uow;
using System;

namespace Sparrow.EntityFrameworkCore.Uow
{
    public static class UowExtensions
    {
        public static TDbContext GetDbContext<TDbContext>(this IActiveUow uow, string name = null)
            where TDbContext : DbContext
        {
            if (uow == null)
            {
                throw new ArgumentNullException(nameof(uow));
            }

            if (!(uow is EfCoreUow))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(EfCoreUow).FullName, nameof(uow));
            }

            return ((EfCoreUow)uow).GetOrCreateDbContext<TDbContext>(name);
        }
    }
}
