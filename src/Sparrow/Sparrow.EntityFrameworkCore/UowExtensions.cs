using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Domain.Uow;
using Sparrow.EntityFrameworkCore.Uow;
using System;

namespace Sparrow.EntityFrameworkCore
{
    public static class UowExtensions
    {
        public static TDbContext GetDbContext<TDbContext>(this IActiveUow uow)
            where TDbContext : DbContext
        {
            if (uow == null)
            {
                throw new ArgumentNullException(nameof(uow));
            }

            if (!(uow is EfCoreUow))
            {
                throw new ArgumentException("uow is not type of " + typeof(EfCoreUow).FullName, nameof(uow));
            }

            return ((EfCoreUow) uow).GetDbContext<TDbContext>();
        }
    }
}
